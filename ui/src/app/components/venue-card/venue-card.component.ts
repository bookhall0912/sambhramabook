import { Component, input, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { Venue } from '../../models/venue.model';

export interface HallCardData extends Venue {
  distance?: string;
  amenities?: string[];
}

interface HallCardDataWithAmenities extends HallCardData {
  amenities: string[];
}

@Component({
  selector: 'app-venue-card',
  imports: [CommonModule, RouterLink],
  templateUrl: './venue-card.component.html',
  styleUrl: './venue-card.component.scss'
})
export class VenueCardComponent {
  venue = input.required<Venue | HallCardData>();
  showBookNow = input<boolean>(false);

  constructor(private router: Router) {}

  // Computed property that ensures amenities is always an array
  venueData = computed<HallCardDataWithAmenities>(() => {
    const data = this.venue() as HallCardData;
    return {
      ...data,
      amenities: data.amenities || []
    };
  });

  formatPrice(price: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR',
      maximumFractionDigits: 0
    }).format(price);
  }

  getVenueData(): HallCardDataWithAmenities {
    return this.venueData();
  }

  onBookNow(event: Event): void {
    event.stopPropagation();
    // Navigate to booking page
    this.router.navigate(['/booking', this.venue().id]);
  }
}
