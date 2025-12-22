import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

interface Transaction {
  id: string;
  date: string;
  bookingId: string;
  amount: number;
  status: 'PAID' | 'PENDING' | 'REFUNDED';
}

@Component({
  selector: 'app-vendor-earnings',
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './earnings.component.html',
  styleUrl: './earnings.component.scss'
})
export class VendorEarningsComponent implements OnInit {
  totalEarnings = signal<number>(452000);
  thisMonth = signal<number>(452000);
  lastMonth = signal<number>(402000);
  pendingBookingsCount = signal<number>(1);
  transactions = signal<Transaction[]>([]);

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    window.scrollTo(0, 0);
    this.loadTransactions();
  }

  private loadTransactions(): void {
    this.transactions.set([
      { id: '1', date: '2024-11-12', bookingId: 'SB-9012', amount: 300000, status: 'PAID' },
      { id: '2', date: '2024-11-08', bookingId: 'SB-9011', amount: 150000, status: 'PAID' },
      { id: '3', date: '2024-11-05', bookingId: 'SB-9010', amount: 200000, status: 'PENDING' },
      { id: '4', date: '2024-10-28', bookingId: 'SB-9009', amount: 180000, status: 'PAID' }
    ]);
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

  onDownloadReport(): void {
    // TODO: Implement download report
    console.log('Download report');
  }

  onLogout(): void {
    this.authService.logout();
  }
}

