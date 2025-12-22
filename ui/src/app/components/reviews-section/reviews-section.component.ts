import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface Review {
  id: string;
  author: string;
  rating: number;
  comment: string;
  date: string;
}

@Component({
  selector: 'app-reviews-section',
  imports: [CommonModule],
  templateUrl: './reviews-section.component.html',
  styleUrl: './reviews-section.component.scss'
})
export class ReviewsSectionComponent {
  reviews = input.required<Review[]>();
}

