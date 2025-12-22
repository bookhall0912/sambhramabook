import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../../services/auth.service';
import { ManageBookingModalComponent, BookingData } from '../../../../components/manage-booking-modal/manage-booking-modal.component';

export interface Booking {
  id: string;
  bookingId: string;
  customerName: string;
  date: string;
  status: 'CONFIRMED' | 'PENDING' | 'CANCELLED';
}

export interface Listing {
  id: string;
  name: string;
  image: string;
  location: string;
  status: 'ACTIVE' | 'DRAFT' | 'INACTIVE';
}

@Component({
  selector: 'app-vendor-overview',
  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive,
    ManageBookingModalComponent
  ],
  templateUrl: './overview.component.html',
  styleUrl: './overview.component.scss'
})
export class VendorOverviewComponent implements OnInit {
  totalEarnings = signal<number>(452000);
  upcomingEvents = signal<number>(8);
  profileViews = signal<number>(1204);
  earningsChange = signal<number>(12.5);
  nextEvent = signal<string>('Priya Weds Rahul (12 Nov)');
  profileViewsChange = signal<number>(5);

  recentBookings = signal<Booking[]>([
    {
      id: '1',
      bookingId: 'SB-9012',
      customerName: 'Amit Verma',
      date: '2024-11-12',
      status: 'CONFIRMED'
    },
    {
      id: '2',
      bookingId: 'SB-9013',
      customerName: 'Sneha Reddy',
      date: '2024-11-15',
      status: 'PENDING'
    },
    {
      id: '3',
      bookingId: 'SB-9014',
      customerName: 'Karthik M',
      date: '2024-11-22',
      status: 'CONFIRMED'
    }
  ]);

  listings = signal<Listing[]>([
    {
      id: '1',
      name: 'Royal Grand Hall',
      image: 'https://images.unsplash.com/photo-1519167758481-83f550bb49b3?w=200',
      location: 'Bangalore',
      status: 'ACTIVE'
    },
    {
      id: '2',
      name: 'Mini Party Hall',
      image: '',
      location: 'Unlisted',
      status: 'DRAFT'
    }
  ]);

  pendingBookingsCount = signal<number>(1);
  showManageModal = signal(false);
  selectedBooking = signal<BookingData | null>(null);

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    window.scrollTo(0, 0);
    
    // Check authentication
    if (!this.authService.isAuthenticated$()) {
      this.router.navigate(['/login'], {
        queryParams: { returnUrl: '/vendor/dashboard' }
      });
      return;
    }

    // Check if user is vendor
    if (this.authService.userRole$() !== 'Vendor') {
      // Redirect to appropriate dashboard
      const role = this.authService.userRole$();
      if (role === 'Admin') {
        this.router.navigate(['/admin/dashboard']);
      } else {
        this.router.navigate(['/']);
      }
      return;
    }

    // Check if vendor profile is complete
    const userData = localStorage.getItem('user_data');
    if (userData) {
      const parsedUser = JSON.parse(userData);
      if (!parsedUser.vendorProfileComplete) {
        // Redirect to onboarding if profile not complete
        this.router.navigate(['/vendor/onboarding']);
        return;
      }
    } else {
      // No user data, go to onboarding
      this.router.navigate(['/vendor/onboarding']);
      return;
    }

    // TODO: Load dashboard data from API
    this.loadDashboardData();
  }

  private loadDashboardData(): void {
    // TODO: Call API to fetch dashboard stats
    // Calculate pending bookings count
    this.pendingBookingsCount.set(
      this.recentBookings().filter(b => b.status === 'PENDING').length
    );
  }

  formatPrice(amount: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR',
      maximumFractionDigits: 0
    }).format(amount);
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-IN', {
      day: 'numeric',
      month: 'short',
      year: 'numeric'
    });
  }

  getStatusClass(status: string): string {
    return `status-${status.toLowerCase()}`;
  }

  onManageBooking(bookingId: string): void {
    const booking = this.recentBookings().find(b => b.id === bookingId);
    if (booking) {
      // Get venue name from listings or use default
      const venueName = this.listings()[0]?.name || 'Royal Grand Hall';
      this.selectedBooking.set({
        id: booking.id,
        bookingId: booking.bookingId,
        venueName: venueName,
        date: booking.date,
        location: 'Bangalore',
        totalAmount: 300000 // Default amount, should come from booking data
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
    this.recentBookings.update(bookings =>
      bookings.map(b => b.id === event.bookingId ? { ...b, status: 'CANCELLED' as const } : b)
    );
    this.loadDashboardData();
  }

  onConfirmReschedule(event: { bookingId: string }): void {
    // TODO: Call API to reschedule booking
    console.log('Reschedule booking:', event);
  }

  onKeepBooking(): void {
    // Modal will close automatically
  }

  onApproveBooking(bookingId: string): void {
    // TODO: Call API to approve booking
    console.log('Approve booking:', bookingId);
    // Update status
    this.recentBookings.update(bookings =>
      bookings.map(b => b.id === bookingId ? { ...b, status: 'CONFIRMED' as const } : b)
    );
    this.loadDashboardData();
  }

  onCreateBooking(): void {
    // TODO: Navigate to create booking page
    console.log('Create booking');
  }

  onAddListing(): void {
    this.router.navigate(['/vendor/dashboard/listings/new']);
  }

  onEditListing(listingId: string): void {
    this.router.navigate(['/vendor/dashboard/listings', listingId, 'edit']);
  }

  onViewAllBookings(): void {
    this.router.navigate(['/vendor/bookings']);
  }

  onLogout(): void {
    this.authService.logout();
  }
}

