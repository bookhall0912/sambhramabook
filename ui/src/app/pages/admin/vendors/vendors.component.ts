import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

export interface Vendor {
  id: string;
  name: string;
  email: string;
  mobile: string;
  avatar: string;
  status: 'ACTIVE' | 'INACTIVE' | 'SUSPENDED';
  joinDate: string;
  listings: number;
  earnings: number;
}

@Component({
  selector: 'app-admin-vendors',
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './vendors.component.html',
  styleUrl: './vendors.component.scss'
})
export class AdminVendorsComponent implements OnInit {
  vendors = signal<Vendor[]>([]);
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
    this.loadVendors();
  }

  private loadVendors(): void {
    this.vendors.set([
      {
        id: '1',
        name: 'Ramesh G',
        email: 'ramesh@example.com',
        mobile: '+91 98765 43210',
        avatar: 'https://i.pravatar.cc/32?img=12',
        status: 'ACTIVE',
        joinDate: '2024-01-10',
        listings: 5,
        earnings: 2500000
      },
      {
        id: '2',
        name: 'Anjali P',
        email: 'anjali@example.com',
        mobile: '+91 98765 43211',
        avatar: 'https://i.pravatar.cc/32?img=22',
        status: 'ACTIVE',
        joinDate: '2024-02-15',
        listings: 3,
        earnings: 1800000
      },
      {
        id: '3',
        name: 'Suresh K',
        email: 'suresh@example.com',
        mobile: '+91 98765 43212',
        avatar: 'https://i.pravatar.cc/32?img=32',
        status: 'INACTIVE',
        joinDate: '2024-03-05',
        listings: 2,
        earnings: 950000
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

  onViewDetails(vendorId: string): void {
    console.log('View vendor details:', vendorId);
  }

  onSuspend(vendorId: string): void {
    this.vendors.update(vendors =>
      vendors.map(v => v.id === vendorId ? { ...v, status: 'SUSPENDED' as const } : v)
    );
  }

  onLogout(): void {
    this.authService.logout();
  }
}

