import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

export interface Payout {
  id: string;
  vendorName: string;
  vendorAvatar: string;
  amount: number;
  period: string;
  status: 'PENDING' | 'PROCESSED' | 'FAILED';
  requestDate: string;
}

@Component({
  selector: 'app-admin-payouts',
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './payouts.component.html',
  styleUrl: './payouts.component.scss'
})
export class AdminPayoutsComponent implements OnInit {
  payouts = signal<Payout[]>([]);
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
    this.loadPayouts();
  }

  private loadPayouts(): void {
    this.payouts.set([
      {
        id: '1',
        vendorName: 'Ramesh G',
        vendorAvatar: 'https://i.pravatar.cc/32?img=12',
        amount: 250000,
        period: 'December 2024',
        status: 'PENDING',
        requestDate: '2024-12-20'
      },
      {
        id: '2',
        vendorName: 'Anjali P',
        vendorAvatar: 'https://i.pravatar.cc/32?img=22',
        amount: 180000,
        period: 'December 2024',
        status: 'PENDING',
        requestDate: '2024-12-21'
      },
      {
        id: '3',
        vendorName: 'Suresh K',
        vendorAvatar: 'https://i.pravatar.cc/32?img=32',
        amount: 150000,
        period: 'November 2024',
        status: 'PROCESSED',
        requestDate: '2024-11-30'
      }
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

  onProcess(payoutId: string): void {
    this.payouts.update(payouts =>
      payouts.map(p => p.id === payoutId ? { ...p, status: 'PROCESSED' as const } : p)
    );
    this.payoutsCount.set(this.payouts().filter(p => p.status === 'PENDING').length);
  }

  onViewDetails(payoutId: string): void {
    console.log('View payout details:', payoutId);
  }

  onLogout(): void {
    this.authService.logout();
  }
}

