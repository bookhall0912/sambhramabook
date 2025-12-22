import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

export interface Booking {
  id: string;
  bookingId: string;
  customerName: string;
  venueName: string;
  date: string;
  amount: number;
  status: 'CONFIRMED' | 'PENDING' | 'CANCELLED';
}

@Component({
  selector: 'app-admin-bookings',
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './bookings.component.html',
  styleUrl: './bookings.component.scss'
})
export class AdminBookingsComponent implements OnInit {
  bookings = signal<Booking[]>([]);
  pendingCount = signal<number>(5);
  payoutsCount = signal<number>(2);

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    window.scrollTo(0, 0);
    if (!this.authService.hasRole('Admin')) {
      this.router.navigate(['/']);
    }
    this.loadBookings();
  }

  private loadBookings(): void {
    this.bookings.set([
      {
        id: '1',
        bookingId: 'BK-2024-001',
        customerName: 'Aditi Sharma',
        venueName: 'Golden Palms Convention Hall',
        date: '2024-12-25',
        amount: 150000,
        status: 'CONFIRMED'
      },
      {
        id: '2',
        bookingId: 'BK-2024-002',
        customerName: 'Rahul Kumar',
        venueName: 'Grand Royal Banquet',
        date: '2024-12-30',
        amount: 200000,
        status: 'PENDING'
      },
      {
        id: '3',
        bookingId: 'BK-2024-003',
        customerName: 'Priya Patel',
        venueName: 'Royal Garden Venue',
        date: '2025-01-05',
        amount: 180000,
        status: 'CONFIRMED'
      }
    ]);
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-IN', {
      day: 'numeric',
      month: 'short',
      year: 'numeric'
    });
  }

  formatPrice(amount: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR',
      maximumFractionDigits: 0
    }).format(amount);
  }

  onViewDetails(bookingId: string): void {
    console.log('View booking details:', bookingId);
  }

  onLogout(): void {
    this.authService.logout();
  }
}

