import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

interface Feature {
  icon: string;
  label: string;
}

@Component({
  selector: 'app-feature-bar',
  imports: [CommonModule],
  templateUrl: './feature-bar.component.html',
  styleUrl: './feature-bar.component.scss'
})
export class FeatureBarComponent {
  features: Feature[] = [
    {
      icon: 'star',
      label: '4.8/5 Average Rating'
    },
    {
      icon: 'verified',
      label: '100% Verified Listings'
    },
    {
      icon: 'map',
      label: '15+ Cities Covered'
    },
    {
      icon: 'shield',
      label: 'Secure Payments & Booking'
    }
  ];
}
