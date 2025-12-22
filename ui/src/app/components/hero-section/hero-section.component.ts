import { Component } from '@angular/core';
import { VenueSearchFormComponent } from '../venue-search-form/venue-search-form.component';

@Component({
  selector: 'app-hero-section',
  imports: [VenueSearchFormComponent],
  templateUrl: './hero-section.component.html',
  styleUrl: './hero-section.component.scss'
})
export class HeroSectionComponent {}
