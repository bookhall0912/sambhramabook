import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface PriceBreakdown {
  basePrice: number;
  addonsTotal: number;
  platformFee: number;
  taxes: number;
  total: number;
}

@Component({
  selector: 'app-booking-summary',
  imports: [CommonModule],
  templateUrl: './booking-summary.component.html',
  styleUrl: './booking-summary.component.scss'
})
export class BookingSummaryComponent {
  hallName = input.required<string>();
  hallImage = input.required<string>();
  location = input.required<string>();
  priceBreakdown = input.required<PriceBreakdown>();

  formatPrice(price: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR',
      maximumFractionDigits: 0
    }).format(price);
  }
}

