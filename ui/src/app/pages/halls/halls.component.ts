import { Component, OnInit, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { HeaderComponent } from '../../components/header/header.component';
import { FooterComponent } from '../../components/footer/footer.component';
import { HallsFiltersComponent, FilterState } from '../../components/halls-filters/halls-filters.component';
import { VenueCardComponent, HallCardData } from '../../components/venue-card/venue-card.component';
import { HallsService } from '../../services/halls.service';

@Component({
  selector: 'app-halls',
  imports: [
    CommonModule,
    HeaderComponent,
    FooterComponent,
    HallsFiltersComponent,
    VenueCardComponent
  ],
  templateUrl: './halls.component.html',
  styleUrl: './halls.component.scss'
})
export class HallsComponent implements OnInit {
  filteredHalls: HallCardData[] = [];
  totalResults = 0;
  location = 'Bangalore';
  radius = 50;
  loading = false;

  constructor(
    private hallsService: HallsService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    effect(() => {
      const state = this.hallsService.hallsList$();
      this.filteredHalls = state.halls;
      this.totalResults = state.total;
      this.loading = state.loading;
    });
  }

  ngOnInit(): void {
    // Scroll to top when component initializes
    window.scrollTo(0, 0);
    
    // Get query params from route
    this.route.queryParams.subscribe(params => {
      const searchRequest = {
        location: params['location'] || 'Bangalore',
        latitude: params['latitude'] ? Number(params['latitude']) : undefined,
        longitude: params['longitude'] ? Number(params['longitude']) : undefined,
        minPrice: params['minPrice'] ? Number(params['minPrice']) : undefined,
        maxPrice: params['maxPrice'] ? Number(params['maxPrice']) : undefined,
        minCapacity: params['minCapacity'] ? Number(params['minCapacity']) : undefined,
        maxCapacity: params['maxCapacity'] ? Number(params['maxCapacity']) : undefined,
        amenities: params['amenities'] ? params['amenities'].split(',') : undefined,
        date: params['date'],
        days: params['days'] ? Number(params['days']) : undefined,
        guests: params['guests'] ? Number(params['guests']) : undefined,
        page: 1,
        pageSize: 12
      };

      this.location = searchRequest.location || 'Bangalore';
      this.hallsService.searchHalls(searchRequest, this.radius);
    });
  }

  onFiltersApplied(filters: FilterState): void {
    // Get current query params from snapshot to preserve location and coordinates
    const params = this.route.snapshot.queryParams;
    const searchRequest = {
      location: params['location'],
      latitude: params['latitude'] ? Number(params['latitude']) : undefined,
      longitude: params['longitude'] ? Number(params['longitude']) : undefined,
      date: params['date'],
      days: params['days'] ? Number(params['days']) : undefined,
      guests: params['guests'] ? Number(params['guests']) : undefined,
      maxPrice: filters.priceRange,
      minCapacity: filters.capacity.length > 0 ? this.parseCapacityRange(filters.capacity[0])?.min : undefined,
      maxCapacity: filters.capacity.length > 0 ? this.parseCapacityRange(filters.capacity[filters.capacity.length - 1])?.max : undefined,
      amenities: filters.amenities,
      page: 1,
      pageSize: 12
    };

    this.hallsService.searchHalls(searchRequest, this.radius);
  }

  private parseCapacityRange(range: string): { min: number; max?: number } | null {
    if (range === '1000+') {
      return { min: 1000 };
    }
    const parts = range.split('-');
    if (parts.length === 2) {
      return { min: Number(parts[0]), max: Number(parts[1]) };
    }
    return null;
  }

  loadMore(): void {
    const currentState = this.hallsService.hallsList$();
    if (currentState.page < currentState.totalPages) {
      // Get current query params to preserve location, coordinates, and filters
      const params = this.route.snapshot.queryParams;
      const searchRequest = {
        location: params['location'],
        latitude: params['latitude'] ? Number(params['latitude']) : undefined,
        longitude: params['longitude'] ? Number(params['longitude']) : undefined,
        date: params['date'],
        days: params['days'] ? Number(params['days']) : undefined,
        guests: params['guests'] ? Number(params['guests']) : undefined,
        minPrice: params['minPrice'] ? Number(params['minPrice']) : undefined,
        maxPrice: params['maxPrice'] ? Number(params['maxPrice']) : undefined,
        minCapacity: params['minCapacity'] ? Number(params['minCapacity']) : undefined,
        maxCapacity: params['maxCapacity'] ? Number(params['maxCapacity']) : undefined,
        amenities: params['amenities'] ? params['amenities'].split(',') : undefined,
        page: currentState.page + 1,
        pageSize: currentState.pageSize
      };
      this.hallsService.searchHalls(searchRequest, this.radius);
    }
  }

  goBack(): void {
    this.router.navigate(['/']);
  }
}
