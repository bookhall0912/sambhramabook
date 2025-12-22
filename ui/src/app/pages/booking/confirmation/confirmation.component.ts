import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, ActivatedRoute } from '@angular/router';
import { HeaderComponent } from '../../../components/header/header.component';
import { FooterComponent } from '../../../components/footer/footer.component';
import { HallsService } from '../../../services/halls.service';
import { AuthService } from '../../../services/auth.service';

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
      // TODO: Fetch booking details from API using bookingId
      this.loadBookingDetails(bookingId, transactionId, paymentMethod, totalAmount);
    } else {
      // For now, use mock data or data from service
      this.loadMockBookingData(transactionId, paymentMethod, totalAmount);
    }
  }

  private loadBookingDetails(bookingId: string, transactionId?: string, paymentMethod?: string, totalAmount?: string): void {
    // TODO: Call API to fetch booking details
    // For now, use mock data with provided payment details
    this.loadMockBookingData(transactionId, paymentMethod, totalAmount, bookingId);
  }

  private loadMockBookingData(transactionId?: string, paymentMethod?: string, totalAmount?: string, bookingId?: string): void {
    // Get user data from auth service for email and phone
    const user = this.authService.currentUser$();
    
    // Mock booking confirmation data
    // In real implementation, this would come from the API response
    const mockData: BookingConfirmationData = {
      bookingId: bookingId || 'SB-8824-X901',
      venueName: 'Sambhrama Grand Hall',
      eventType: 'Wedding Reception',
      startDate: '2024-11-12',
      endDate: '2024-11-13',
      days: 2,
      guestCount: 800,
      totalAmount: totalAmount ? parseInt(totalAmount, 10) : 306425,
      transactionId: transactionId || '88291039912',
      paymentMethod: paymentMethod || 'UPI',
      email: user?.email || 'user@example.com',
      phone: user?.mobile || '+91 98765 43210',
      status: 'PAID & CONFIRMED'
    };

    this.bookingData.set(mockData);
    this.loading.set(false);
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

