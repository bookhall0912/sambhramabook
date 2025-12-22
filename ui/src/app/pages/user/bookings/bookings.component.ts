import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { ManageBookingModalComponent, BookingData } from '../../../components/manage-booking-modal/manage-booking-modal.component';

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
  
  bookings = signal<Booking[]>([
    {
      id: '1',
      referenceId: 'SB-8824-X901',
      venueName: 'Sambhrama Grand Convention Hall',
      venueImage: 'https://images.unsplash.com/photo-1528605248644-14dd04022da1',
      location: 'JP Nagar, Bangalore',
      startDate: '2024-11-12',
      endDate: '2024-11-13',
      days: 2,
      guestCount: 800,
      totalAmount: 306425,
      status: 'UPCOMING',
      paymentStatus: 'PAID',
      eventType: 'Wedding Reception'
    },
    {
      id: '2',
      referenceId: 'SB-9932-P102',
      venueName: 'Shutterbug Photography',
      venueImage: '',
      location: 'Bangalore',
      startDate: '2024-11-12',
      endDate: '2024-11-12',
      days: 1,
      guestCount: 0,
      totalAmount: 25000,
      status: 'UPCOMING',
      paymentStatus: 'PENDING',
      eventType: 'Candid Photography'
    }
  ]);

  savedVenues = signal<SavedVenue[]>([
    {
      id: '1',
      name: 'Royal Orchid Hall',
      image: '',
      location: 'Indiranagar, Bangalore'
    },
    {
      id: '2',
      name: 'Golden Petal Events',
      image: 'https://images.unsplash.com/photo-1511795409834-ef04bbd61622',
      location: 'Bangalore',
      rating: 4.9,
      reviewCount: 120
    }
  ]);

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

    // TODO: Load bookings from API based on active tab
    this.loadBookings();
  }

  private loadBookings(): void {
    // TODO: Call API to fetch bookings based on activeTab
    // For now, using mock data
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
    console.log('Cancel booking:', event);
    // TODO: Call API to cancel booking
    // Update booking status to CANCELLED
    this.bookings.update(bookings => 
      bookings.map(b => 
        b.id === event.bookingId 
          ? { ...b, status: 'CANCELLED' as const, paymentStatus: 'REFUNDED' as const }
          : b
      )
    );
    this.onCloseModal();
  }

  onConfirmReschedule(event: { bookingId: string }): void {
    console.log('Reschedule booking:', event);
    // TODO: Call API to request reschedule
    // TODO: Navigate to reschedule page or show reschedule form
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

