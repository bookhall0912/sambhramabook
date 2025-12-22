import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

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
  selector: 'app-admin-listings-approval',
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './listings-approval.component.html',
  styleUrl: './listings-approval.component.scss'
})
export class AdminListingsApprovalComponent implements OnInit {
  listings = signal<PendingListing[]>([]);
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
    this.loadListings();
  }

  private loadListings(): void {
    this.listings.set([
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
      },
      {
        id: '4',
        name: 'Catering Services Pro',
        location: 'Koramangala, Bangalore',
        vendorName: 'Priya M',
        vendorAvatar: 'https://i.pravatar.cc/28?img=42',
        type: 'Service',
        submitted: '2 days ago',
        status: 'PENDING'
      },
      {
        id: '5',
        name: 'Royal Garden Venue',
        location: 'Whitefield, Bangalore',
        vendorName: 'Karthik R',
        vendorAvatar: 'https://i.pravatar.cc/28?img=52',
        type: 'Hall',
        submitted: '3 days ago',
        status: 'PENDING'
      }
    ]);
  }

  onApprove(listingId: string): void {
    this.listings.update(listings => listings.filter(l => l.id !== listingId));
    this.pendingCount.set(this.listings().filter(l => l.status === 'PENDING').length);
  }

  onReject(listingId: string): void {
    this.listings.update(listings => listings.filter(l => l.id !== listingId));
    this.pendingCount.set(this.listings().filter(l => l.status === 'PENDING').length);
  }

  onRequestChanges(listingId: string): void {
    this.listings.update(listings =>
      listings.map(l => l.id === listingId ? { ...l, status: 'NEEDS_CHANGES' as const } : l)
    );
  }

  onViewDetails(listingId: string): void {
    console.log('View listing details:', listingId);
  }

  onLogout(): void {
    this.authService.logout();
  }
}

