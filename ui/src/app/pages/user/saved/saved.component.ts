import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { UserApiService } from '../../../services/user.api';
import { catchError, of } from 'rxjs';

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
  savedVenues = signal<SavedVenue[]>([]);
  private userApiService = inject(UserApiService);

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    window.scrollTo(0, 0);
    this.loadSavedVenues();
  }

  private loadSavedVenues(): void {
    this.userApiService.getSavedListings()
      .pipe(
        catchError(error => {
          console.error('Error loading saved venues:', error);
          return of({ success: false, data: [] });
        })
      )
      .subscribe(response => {
        if (response.success) {
          const saved = response.data.map(s => ({
            id: s.id,
            name: s.title,
            image: s.imageUrl || '',
            location: s.location,
            rating: s.rating,
            reviewCount: s.reviewCount,
            price: s.price,
            capacity: s.capacity
          }));
          this.savedVenues.set(saved);
        }
      });
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
    this.userApiService.removeSavedListing(venueId)
      .pipe(
        catchError(error => {
          console.error('Error removing saved venue:', error);
          alert('Failed to remove venue from saved list.');
          return of(null);
        })
      )
      .subscribe(response => {
        if (response) {
          this.savedVenues.update(venues => venues.filter(v => v.id !== venueId));
        }
      });
  }

  onLogout(): void {
    this.authService.logout();
  }
}

