import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, ActivatedRoute } from '@angular/router';
import { HeaderComponent } from '../../../components/header/header.component';
import { FooterComponent } from '../../../components/footer/footer.component';
import { HallsService } from '../../../services/halls.service';
import { AuthService } from '../../../services/auth.service';
import { BookingsApiService, BookingDetailDto } from '../../../services/bookings.api';
import { catchError, of } from 'rxjs';

interface BookingConfirmationData {
  bookingId: string;
  venueName: string;
  eventType: string;
  startDate: string;
  endDate: string;
  days: number;
  guestCount: number;
  totalAmount: number;
  transactionId: string;
  paymentMethod: string;
  email: string;
  phone: string;
  status: 'PAID & CONFIRMED' | 'PENDING' | 'CANCELLED';
}

@Component({
  selector: 'app-booking-confirmation',
  imports: [
    CommonModule,
    RouterLink,
    HeaderComponent,
    FooterComponent
  ],
  templateUrl: './confirmation.component.html',
  styleUrl: './confirmation.component.scss'
})
export class BookingConfirmationComponent implements OnInit {
  bookingData = signal<BookingConfirmationData | null>(null);
  loading = signal<boolean>(true);
  private bookingsApiService = inject(BookingsApiService);

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private hallsService: HallsService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    // Scroll to top when component initializes
    window.scrollTo(0, 0);

    // Get booking ID and payment details from route params or query params
    const bookingId = this.route.snapshot.paramMap.get('id') || 
                     this.route.snapshot.queryParams['bookingId'];
    const transactionId = this.route.snapshot.queryParams['transactionId'];
    const paymentMethod = this.route.snapshot.queryParams['paymentMethod'];
    const totalAmount = this.route.snapshot.queryParams['totalAmount'];

    if (bookingId) {
      // Fetch booking details from API using bookingId
      this.loadBookingDetails(bookingId, transactionId, paymentMethod, totalAmount);
    } else {
      // Fallback to query params if no booking ID
      this.loadBookingFromQueryParams(transactionId, paymentMethod, totalAmount);
    }
  }

  private loadBookingDetails(bookingId: string, transactionId?: string, paymentMethod?: string, totalAmount?: string): void {
    // Try to get booking by ID first, then by reference
    const isNumericId = /^\d+$/.test(bookingId);
    const booking$ = isNumericId
      ? this.bookingsApiService.getBookingById(bookingId)
      : this.bookingsApiService.getBookingByReference(bookingId);

    booking$
      .pipe(
        catchError(error => {
          console.error('Error fetching booking details:', error);
          // Fallback to query params if API fails
          return of(null);
        })
      )
      .subscribe(booking => {
        if (booking) {
          this.bookingData.set({
            bookingId: booking.bookingId || booking.referenceId,
            venueName: booking.venueName,
            eventType: booking.eventType || 'Event',
            startDate: booking.startDate,
            endDate: booking.endDate,
            days: booking.days,
            guestCount: booking.guestCount,
            totalAmount: booking.totalAmount,
            transactionId: booking.paymentTransactionId || transactionId || '',
            paymentMethod: booking.paymentMethod || paymentMethod || 'UPI',
            email: booking.contactInfo?.email || this.authService.currentUser$()?.email || '',
            phone: booking.contactInfo?.phone || this.authService.currentUser$()?.mobile || '',
            status: booking.paymentStatus === 'PAID' ? 'PAID & CONFIRMED' : 
                    booking.status === 'CANCELLED' ? 'CANCELLED' : 'PENDING'
          });
        } else {
          // Fallback to query params if API fails
          this.loadBookingFromQueryParams(transactionId, paymentMethod, totalAmount, bookingId);
        }
        this.loading.set(false);
      });
  }

  private loadBookingFromQueryParams(transactionId?: string, paymentMethod?: string, totalAmount?: string, bookingId?: string): void {
    // Get user data from auth service for email and phone
    const user = this.authService.currentUser$();
    
    // Use query params as fallback
    this.bookingData.set({
      bookingId: bookingId || 'SB-8824-X901',
      venueName: 'Venue',
      eventType: 'Event',
      startDate: new Date().toISOString().split('T')[0],
      endDate: new Date().toISOString().split('T')[0],
      days: 1,
      guestCount: 0,
      totalAmount: totalAmount ? parseInt(totalAmount, 10) : 0,
      transactionId: transactionId || '',
      paymentMethod: paymentMethod || 'UPI',
      email: user?.email || '',
      phone: user?.mobile || '',
      status: 'PENDING'
    });
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-IN', {
      day: 'numeric',
      month: 'short',
      year: 'numeric'
    });
  }

  formatDateRange(startDate: string, endDate: string, days: number): string {
    const start = this.formatDate(startDate);
    const end = this.formatDate(endDate);
    return `${start} – ${end} (${days} ${days === 1 ? 'Day' : 'Days'})`;
  }

  formatPrice(amount: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR',
      maximumFractionDigits: 0
    }).format(amount);
  }

  getStatusClass(status: string): string {
    // Convert status to CSS class format (e.g., "PAID & CONFIRMED" -> "status-paid-confirmed")
    return 'status-' + status.toLowerCase().replace(/\s+/g, '-').replace(/&/g, '');
  }

  onViewBookings(): void {
    // TODO: Navigate to user's bookings page
    this.router.navigate(['/bookings']);
  }

  onDownloadInvoice(): void {
    // TODO: Generate and download invoice PDF
    console.log('Downloading invoice for booking:', this.bookingData()?.bookingId);
    // In real implementation, call API to generate PDF
  }

  onContactManager(): void {
    // TODO: Open contact form or initiate chat
    console.log('Contacting venue manager');
  }
}

