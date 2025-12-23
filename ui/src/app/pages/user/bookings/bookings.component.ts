import { Component, OnInit, signal, WritableSignal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { UserApiService } from '../../../services/user.api';
import { BookingsApiService } from '../../../services/bookings.api';
import { ManageBookingModalComponent, BookingData } from '../../../components/manage-booking-modal/manage-booking-modal.component';
import { catchError, of } from 'rxjs';

export interface Booking {
  id: string;
  referenceId: string;
  venueName: string;
  venueImage: string;
  location: string;
  startDate: string;
  endDate: string;
  days: number;
  guestCount: number;
  totalAmount: number;
  status: 'UPCOMING' | 'PAST' | 'CANCELLED' | 'PENDING_CONFIRMATION';
  paymentStatus: 'PAID' | 'PENDING' | 'REFUNDED';
  eventType?: string;
}

export interface SavedVenue {
  id: string;
  name: string;
  image: string;
  location: string;
  rating?: number;
  reviewCount?: number;
}

type TabType = 'UPCOMING' | 'PAST' | 'CANCELLED';

@Component({
  selector: 'app-bookings',
  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive,
    ManageBookingModalComponent
  ],
  templateUrl: './bookings.component.html',
  styleUrl: './bookings.component.scss'
})
export class BookingsComponent implements OnInit {
  activeTab = signal<TabType>('UPCOMING');
  userName = signal<string>('Aditi');
  showManageModal = signal<boolean>(false);
  selectedBooking = signal<BookingData | null>(null);
  
  bookings = signal<Booking[]>([]);
  savedVenues = signal<SavedVenue[]>([]);

  private userApiService = inject(UserApiService);
  private bookingsApiService = inject(BookingsApiService);

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    window.scrollTo(0, 0);
    
    // Get user name from auth service
    const user = this.authService.currentUser$();
    if (user?.name) {
      this.userName.set(user.name);
    }

    // Load bookings from API based on active tab
    this.loadBookings();
    // Load saved venues
    this.loadSavedVenues();
  }

  private loadSavedVenues(): void {
    this.userApiService.getSavedListings()
      .pipe(
        catchError(error => {
          console.error('Error loading saved venues:', error);
          return of({ success: false, data: [] });
        })
      )
      .subscribe(response => {
        if (response.success) {
          const saved = response.data.map(s => ({
            id: s.id,
            name: s.title,
            image: s.imageUrl || '',
            location: s.location,
            rating: s.rating,
            reviewCount: s.reviewCount
          }));
          this.savedVenues.set(saved);
        }
      });
  }

  private loadBookings(): void {
    const status = this.activeTab() === 'UPCOMING' ? 'UPCOMING' : 
                   this.activeTab() === 'PAST' ? 'PAST' : 'CANCELLED';
    
    this.userApiService.getUserBookings(status, 1, 20)
      .pipe(
        catchError(error => {
          console.error('Error loading bookings:', error);
          return of({ success: false, data: [] });
        })
      )
      .subscribe(response => {
        if (response.success) {
          const bookings = response.data.map(b => ({
            id: b.id,
            referenceId: b.referenceId,
            venueName: b.venueName,
            venueImage: b.venueImage || '',
            location: b.location,
            startDate: b.startDate,
            endDate: b.endDate,
            days: b.days,
            guestCount: b.guestCount,
            totalAmount: b.totalAmount,
            status: b.status,
            paymentStatus: b.paymentStatus,
            eventType: b.eventType
          }));
          this.bookings.set(bookings);
        }
      });
  }

  setActiveTab(tab: TabType): void {
    this.activeTab.set(tab);
    this.loadBookings();
  }

  getFilteredBookings(): Booking[] {
    return this.bookings().filter(booking => {
      switch (this.activeTab()) {
        case 'UPCOMING':
          return booking.status === 'UPCOMING' || booking.status === 'PENDING_CONFIRMATION';
        case 'PAST':
          return booking.status === 'PAST';
        case 'CANCELLED':
          return booking.status === 'CANCELLED';
        default:
          return false;
      }
    });
  }

  formatDateRange(startDate: string, endDate: string, days: number): string {
    const start = new Date(startDate).toLocaleDateString('en-IN', {
      day: 'numeric',
      month: 'short',
      year: 'numeric'
    });
    const end = new Date(endDate).toLocaleDateString('en-IN', {
      day: 'numeric',
      month: 'short',
      year: 'numeric'
    });
    
    if (days === 1) {
      return `${start}`;
    }
    return `${start} – ${end}`;
  }

  formatPrice(amount: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR',
      maximumFractionDigits: 0
    }).format(amount);
  }

  getStatusBadgeClass(booking: Booking): string {
    if (booking.status === 'PENDING_CONFIRMATION') {
      return 'pending';
    }
    if (booking.paymentStatus === 'PAID') {
      return 'upcoming';
    }
    return 'pending';
  }

  getStatusText(booking: Booking): string {
    if (booking.status === 'PENDING_CONFIRMATION') {
      return 'PENDING CONFIRMATION';
    }
    if (booking.paymentStatus === 'PAID') {
      return 'UPCOMING · PAID';
    }
    return 'UPCOMING';
  }

  onViewDetails(bookingId: string): void {
    // TODO: Navigate to booking details page
    this.router.navigate(['/booking/confirmation'], {
      queryParams: { bookingId }
    });
  }

  onViewInvoice(bookingId: string): void {
    // TODO: Download or view invoice
    console.log('View invoice for booking:', bookingId);
  }

  onContactVenue(bookingId: string): void {
    // TODO: Open contact form or chat
    console.log('Contact venue for booking:', bookingId);
  }

  onManageBooking(booking: Booking): void {
    // Check if booking is eligible for cancellation/rescheduling
    // Only allow for UPCOMING bookings that are PAID
    if (booking.status === 'UPCOMING' && booking.paymentStatus === 'PAID') {
      const bookingData: BookingData = {
        id: booking.id,
        bookingId: booking.referenceId,
        venueName: booking.venueName,
        venueImage: booking.venueImage,
        date: booking.startDate,
        location: booking.location,
        totalAmount: booking.totalAmount,
        cancellationFee: booking.totalAmount * 0.5, // 50% cancellation fee
        estimatedRefund: booking.totalAmount * 0.5
      };
      this.selectedBooking.set(bookingData);
      this.showManageModal.set(true);
    }
  }

  onCloseModal(): void {
    this.showManageModal.set(false);
    this.selectedBooking.set(null);
  }

  onConfirmCancel(event: { bookingId: string; reason: string }): void {
    const booking = this.bookings().find(b => b.id === event.bookingId);
    if (!booking) return;

    if (confirm(`Are you sure you want to cancel booking ${booking.referenceId}?`)) {
      this.bookingsApiService.cancelBooking(event.bookingId)
        .pipe(
          catchError(error => {
            console.error('Error cancelling booking:', error);
            alert('Failed to cancel booking. Please try again.');
            return of(null);
          })
        )
        .subscribe(response => {
          if (response) {
            alert('Booking cancelled successfully.');
            this.loadBookings(); // Reload bookings
          }
        });
    }
    this.onCloseModal();
  }

  onConfirmReschedule(event: { bookingId: string }): void {
    // TODO: Navigate to reschedule page or show reschedule form
    // For now, just show a message
    alert('Reschedule functionality will open a form to select new dates.');
    this.onCloseModal();
  }

  onKeepBooking(): void {
    this.onCloseModal();
  }

  isEligibleForManagement(booking: Booking): boolean {
    // Only UPCOMING and PAID bookings can be cancelled/rescheduled
    return booking.status === 'UPCOMING' && booking.paymentStatus === 'PAID';
  }

  onViewSavedVenue(venueId: string): void {
    // TODO: Navigate to venue detail page
    this.router.navigate(['/halls', venueId]);
  }

  onViewAllSaved(): void {
    this.router.navigate(['/bookings/saved']);
  }

  onLogout(): void {
    this.authService.logout();
  }
}

