import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { VendorApiService } from '../../../services/vendor.api';
import { ManageBookingModalComponent, BookingData } from '../../../components/manage-booking-modal/manage-booking-modal.component';
import { catchError, of } from 'rxjs';

export interface VendorBooking {
  id: string;
  bookingId: string;
  customerName: string;
  customerPhone: string;
  venueName: string;
  date: string;
  days: number;
  guests: number;
  amount: number;
  status: 'CONFIRMED' | 'PENDING' | 'CANCELLED';
}

@Component({
  selector: 'app-vendor-bookings',
  imports: [CommonModule, RouterLink, RouterLinkActive, ManageBookingModalComponent],
  templateUrl: './bookings.component.html',
  styleUrl: './bookings.component.scss'
})
export class VendorBookingsComponent implements OnInit {
  bookings = signal<VendorBooking[]>([]);
  selectedFilter = signal<'all' | 'pending' | 'confirmed'>('all');
  pendingBookingsCount = signal<number>(0);
  showManageModal = signal(false);
  selectedBooking = signal<BookingData | null>(null);

  filteredBookings = computed(() => {
    const filter = this.selectedFilter();
    if (filter === 'all') return this.bookings();
    return this.bookings().filter(b => b.status.toLowerCase() === filter.toUpperCase());
  });

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    window.scrollTo(0, 0);
    this.loadBookings();
  }

  private vendorApiService = inject(VendorApiService);

  private loadBookings(): void {
    const status = this.selectedFilter() === 'all' ? 'all' : 
                   this.selectedFilter() === 'pending' ? 'pending' : 'confirmed';
    
    this.vendorApiService.getVendorBookings(status)
      .pipe(
        catchError(error => {
          console.error('Error loading vendor bookings:', error);
          return of({ success: false, data: [] });
        })
      )
      .subscribe(response => {
        if (response.success) {
          const bookings = response.data.map(b => ({
            id: b.id,
            bookingId: b.bookingId,
            customerName: b.customerName,
            customerPhone: b.customerPhone,
            venueName: b.venueName,
            date: b.date,
            days: b.days,
            guests: b.guests,
            amount: b.amount,
            status: b.status
          }));
          this.bookings.set(bookings);
          this.pendingBookingsCount.set(bookings.filter(b => b.status === 'PENDING').length);
        }
      });
  }

  setFilter(filter: 'all' | 'pending' | 'confirmed'): void {
    this.selectedFilter.set(filter);
  }

  formatPrice(amount: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR',
      maximumFractionDigits: 0
    }).format(amount);
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-IN', {
      day: 'numeric',
      month: 'short',
      year: 'numeric'
    });
  }

  onApprove(id: string): void {
    this.vendorApiService.approveBooking(id)
      .pipe(
        catchError(error => {
          console.error('Error approving booking:', error);
          alert('Failed to approve booking. Please try again.');
          return of(null);
        })
      )
      .subscribe(response => {
        if (response) {
          this.loadBookings(); // Reload bookings
        }
      });
  }

  onManage(id: string): void {
    const booking = this.bookings().find(b => b.id === id);
    if (booking) {
      this.selectedBooking.set({
        id: booking.id,
        bookingId: booking.bookingId,
        venueName: booking.venueName,
        date: booking.date,
        location: 'Bangalore',
        totalAmount: booking.amount
      });
      this.showManageModal.set(true);
    }
  }

  onCloseModal(): void {
    this.showManageModal.set(false);
    this.selectedBooking.set(null);
  }

  onConfirmCancel(event: { bookingId: string; reason: string }): void {
    this.vendorApiService.rejectBooking(event.bookingId, event.reason)
      .pipe(
        catchError(error => {
          console.error('Error cancelling booking:', error);
          alert('Failed to cancel booking. Please try again.');
          return of(null);
        })
      )
      .subscribe(response => {
        if (response) {
          this.loadBookings(); // Reload bookings
        }
      });
  }

  onConfirmReschedule(event: { bookingId: string }): void {
    // TODO: Implement reschedule with date selection
    alert('Reschedule functionality will open a form to select new dates.');
  }

  onKeepBooking(): void {
    // Modal will close automatically
  }

  onLogout(): void {
    this.authService.logout();
  }
}

