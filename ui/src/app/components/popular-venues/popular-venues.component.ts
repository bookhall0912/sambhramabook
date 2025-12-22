import { Component, OnInit, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { VenueCardComponent } from '../venue-card/venue-card.component';
import { HallsService } from '../../services/halls.service';
import { HallCardData } from '../venue-card/venue-card.component';

@Component({
  selector: 'app-popular-venues',
  imports: [CommonModule, RouterLink, VenueCardComponent],
  templateUrl: './popular-venues.component.html',
  styleUrl: './popular-venues.component.scss'
})
export class PopularVenuesComponent implements OnInit {
  venues: HallCardData[] = [];

  constructor(private hallsService: HallsService) {
    effect(() => {
      this.venues = this.hallsService.popularHalls$();
    });
  }

  ngOnInit(): void {
    this.hallsService.fetchPopularHalls(3);
  }
}
