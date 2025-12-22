import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { ManageBookingModalComponent, BookingData } from '../../../components/manage-booking-modal/manage-booking-modal.component';

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

  private loadBookings(): void {
    // Mock data
    const allBookings: VendorBooking[] = [
      {
        id: '1',
        bookingId: 'SB-9012',
        customerName: 'Amit Verma',
        customerPhone: '+91 98765 43210',
        venueName: 'Royal Grand Hall',
        date: '2024-11-12',
        days: 2,
        guests: 500,
        amount: 300000,
        status: 'CONFIRMED'
      },
      {
        id: '2',
        bookingId: 'SB-9013',
        customerName: 'Sneha Reddy',
        customerPhone: '+91 98765 43211',
        venueName: 'Royal Grand Hall',
        date: '2024-11-15',
        days: 1,
        guests: 300,
        amount: 150000,
        status: 'PENDING'
      },
      {
        id: '3',
        bookingId: 'SB-9014',
        customerName: 'Karthik M',
        customerPhone: '+91 98765 43212',
        venueName: 'Royal Grand Hall',
        date: '2024-11-22',
        days: 1,
        guests: 400,
        amount: 200000,
        status: 'CONFIRMED'
      },
      {
        id: '4',
        bookingId: 'SB-9015',
        customerName: 'Priya Sharma',
        customerPhone: '+91 98765 43213',
        venueName: 'Royal Grand Hall',
        date: '2024-12-01',
        days: 3,
        guests: 600,
        amount: 450000,
        status: 'PENDING'
      }
    ];
    this.bookings.set(allBookings);
    this.pendingBookingsCount.set(allBookings.filter(b => b.status === 'PENDING').length);
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
    // TODO: Call API
    this.bookings.update(bookings =>
      bookings.map(b => b.id === id ? { ...b, status: 'CONFIRMED' as const } : b)
    );
    this.pendingBookingsCount.set(this.bookings().filter(b => b.status === 'PENDING').length);
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
    // TODO: Call API to cancel booking
    console.log('Cancel booking:', event);
    this.bookings.update(bookings =>
      bookings.map(b => b.id === event.bookingId ? { ...b, status: 'CANCELLED' as const } : b)
    );
  }

  onConfirmReschedule(event: { bookingId: string }): void {
    // TODO: Call API to reschedule booking
    console.log('Reschedule booking:', event);
  }

  onKeepBooking(): void {
    // Modal will close automatically
  }

  onLogout(): void {
    this.authService.logout();
  }
}

