import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

export interface SavedVenue {
  id: string;
  name: string;
  image: string;
  location: string;
  rating?: number;
  reviewCount?: number;
  price?: number;
  capacity?: number;
}

@Component({
  selector: 'app-saved',
  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './saved.component.html',
  styleUrl: './saved.component.scss'
})
export class SavedComponent implements OnInit {
  savedVenues = signal<SavedVenue[]>([
    {
      id: '1',
      name: 'Royal Orchid Hall',
      image: 'https://images.unsplash.com/photo-1511795409834-ef04bbd61622',
      location: 'Indiranagar, Bangalore',
      rating: 4.8,
      reviewCount: 245,
      price: 150000,
      capacity: 500
    },
    {
      id: '2',
      name: 'Golden Petal Events',
      image: 'https://images.unsplash.com/photo-1511795409834-ef04bbd61622',
      location: 'Whitefield, Bangalore',
      rating: 4.9,
      reviewCount: 120,
      price: 200000,
      capacity: 800
    },
    {
      id: '3',
      name: 'Grandeur Convention Center',
      image: 'https://images.unsplash.com/photo-1519167758481-83f550bb49b3',
      location: 'Koramangala, Bangalore',
      rating: 4.7,
      reviewCount: 189,
      price: 180000,
      capacity: 600
    }
  ]);

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    window.scrollTo(0, 0);
    // TODO: Load saved venues from API
  }

  formatPrice(amount: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR',
      maximumFractionDigits: 0
    }).format(amount);
  }

  onViewDetails(venueId: string): void {
    this.router.navigate(['/halls', venueId]);
  }

  onRemoveSaved(venueId: string): void {
    // TODO: Call API to remove from saved list
    this.savedVenues.update(venues => venues.filter(v => v.id !== venueId));
  }

  onLogout(): void {
    this.authService.logout();
  }
}

