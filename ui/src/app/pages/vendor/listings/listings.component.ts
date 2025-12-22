import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

export interface Listing {
  id: string;
  name: string;
  image: string;
  location: string;
  status: 'ACTIVE' | 'DRAFT' | 'INACTIVE';
  price?: number;
  capacity?: number;
  views?: number;
  bookings?: number;
}

@Component({
  selector: 'app-vendor-listings',
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './listings.component.html',
  styleUrl: './listings.component.scss'
})
export class VendorListingsComponent implements OnInit {
  listings = signal<Listing[]>([]);
  pendingBookingsCount = signal<number>(1);

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    window.scrollTo(0, 0);
    // TODO: Load listings from API
    this.loadListings();
  }

  private loadListings(): void {
    // Mock data
    this.listings.set([
      {
        id: '1',
        name: 'Royal Grand Hall',
        image: '',
        location: 'Bangalore',
        status: 'ACTIVE',
        price: 150000,
        capacity: 500,
        views: 1204,
        bookings: 8
      },
      {
        id: '2',
        name: 'Mini Party Hall',
        image: '',
        location: 'Bangalore',
        status: 'DRAFT',
        price: 80000,
        capacity: 200
      }
    ]);
  }

  onAddNew(): void {
    this.router.navigate(['/vendor/dashboard/listings/new']);
  }

  formatPrice(amount: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR',
      maximumFractionDigits: 0
    }).format(amount);
  }

  onEdit(id: string): void {
    this.router.navigate(['/vendor/dashboard/listings', id, 'edit']);
  }

  onLogout(): void {
    this.authService.logout();
  }
}

