import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../../services/auth.service';

export interface PendingListing {
  id: string;
  name: string;
  location: string;
  vendorName: string;
  vendorAvatar: string;
  type: 'Hall' | 'Service';
  submitted: string;
  status: 'PENDING' | 'NEEDS_CHANGES';
}

@Component({
  selector: 'app-admin-overview',
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './overview.component.html',
  styleUrl: './overview.component.scss'
})
export class AdminOverviewComponent implements OnInit {
  totalUsers = signal<number>(24582);
  activeVendors = signal<number>(1240);
  totalBookings = signal<number>(8432);
  platformRevenue = signal<number>(42500000);
  
  usersChange = signal<number>(12);
  vendorsChange = signal<number>(8);
  bookingsChange = signal<number>(24);
  revenueChange = signal<number>(18);

  pendingListings = signal<PendingListing[]>([
    {
      id: '1',
      name: 'Golden Palms Convention Hall',
      location: 'JP Nagar, Bangalore',
      vendorName: 'Ramesh G',
      vendorAvatar: 'https://i.pravatar.cc/28?img=12',
      type: 'Hall',
      submitted: '2 hours ago',
      status: 'PENDING'
    },
    {
      id: '2',
      name: 'Shutterbug Photography',
      location: 'Indiranagar, Bangalore',
      vendorName: 'Anjali P',
      vendorAvatar: 'https://i.pravatar.cc/28?img=22',
      type: 'Service',
      submitted: '5 hours ago',
      status: 'PENDING'
    },
    {
      id: '3',
      name: 'Grand Royal Banquet',
      location: 'Mysore Road',
      vendorName: 'Suresh K',
      vendorAvatar: 'https://i.pravatar.cc/28?img=32',
      type: 'Hall',
      submitted: '1 day ago',
      status: 'NEEDS_CHANGES'
    }
  ]);

  pendingCount = signal<number>(5);
  payoutsCount = signal<number>(2);

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    window.scrollTo(0, 0);
    const isAuthenticated = this.authService.isAuthenticated$();
    const userRole = this.authService.userRole$();
    
    if (!isAuthenticated) {
      // Not logged in - redirect to login with returnUrl
      this.router.navigate(['/login'], { queryParams: { returnUrl: '/admin/dashboard' } });
      return;
    }
    
    if (userRole !== 'Admin') {
      // Logged in but not Admin - redirect to their dashboard or home
      if (userRole === 'Vendor') {
        this.router.navigate(['/vendor/dashboard']);
      } else {
        this.router.navigate(['/']);
      }
      return;
    }
    
    // User is authenticated and is Admin - allow access
  }

  formatRevenue(amount: number): string {
    if (amount >= 10000000) {
      return `₹ ${(amount / 10000000).toFixed(1)} Cr`;
    }
    return `₹ ${(amount / 100000).toFixed(1)} L`;
  }

  onReview(listingId: string): void {
    // TODO: Navigate to review page
    console.log('Review listing:', listingId);
  }

  onReject(listingId: string): void {
    // TODO: Call API to reject listing
    console.log('Reject listing:', listingId);
  }

  onViewDetails(listingId: string): void {
    // TODO: Navigate to listing details
    console.log('View details:', listingId);
  }

  onViewAll(): void {
    this.router.navigate(['/admin/listings-approval']);
  }

  onLogout(): void {
    this.authService.logout();
  }
}

