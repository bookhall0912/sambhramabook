import { Component, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

export interface FilterState {
  priceRange: number;
  capacity: string[];
  amenities: string[];
}

@Component({
  selector: 'app-halls-filters',
  imports: [CommonModule, FormsModule],
  templateUrl: './halls-filters.component.html',
  styleUrl: './halls-filters.component.scss'
})
export class HallsFiltersComponent {
  filtersApplied = output<FilterState>();

  priceRange = 500000;
  maxPrice = 500000;

  capacityOptions = [
    { value: '300-500', label: '300 - 500' },
    { value: '500-1000', label: '500 - 1000' },
    { value: '1000+', label: '1000+' }
  ];

  amenityOptions = [
    { value: 'ac', label: 'Air Conditioned' },
    { value: 'parking', label: 'Valet Parking' },
    { value: 'rooms', label: 'Guest Rooms' }
  ];

  selectedCapacity: string[] = [];
  selectedAmenities: string[] = [];

  onCapacityChange(value: string, checked: boolean): void {
    if (checked) {
      this.selectedCapacity.push(value);
    } else {
      this.selectedCapacity = this.selectedCapacity.filter(c => c !== value);
    }
  }

  onAmenityChange(value: string, checked: boolean): void {
    if (checked) {
      this.selectedAmenities.push(value);
    } else {
      this.selectedAmenities = this.selectedAmenities.filter(a => a !== value);
    }
  }

  applyFilters(): void {
    this.filtersApplied.emit({
      priceRange: this.priceRange,
      capacity: this.selectedCapacity,
      amenities: this.selectedAmenities
    });
  }
}

