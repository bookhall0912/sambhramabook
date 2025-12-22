import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface Addon {
  id: string;
  title: string;
  description: string;
  price: number;
  selected: boolean;
}

@Component({
  selector: 'app-addon-card',
  imports: [CommonModule],
  templateUrl: './addon-card.component.html',
  styleUrl: './addon-card.component.scss'
})
export class AddonCardComponent {
  addon = input.required<Addon>();
  toggle = output<void>();

  onToggle(): void {
    this.toggle.emit();
  }

  formatPrice(price: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR',
      maximumFractionDigits: 0
    }).format(price);
  }
}

