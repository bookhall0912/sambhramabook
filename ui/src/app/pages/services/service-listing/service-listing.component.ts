import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { HeaderComponent } from '../../../components/header/header.component';
import { FooterComponent } from '../../../components/footer/footer.component';

export interface ServiceVendor {
  id: string;
  name: string;
  image?: string;
  rating: number;
  reviewCount: number;
  description: string;
  price: number;
  priceUnit: string;
  tag: string;
}

@Component({
  selector: 'app-service-listing',
  imports: [CommonModule, HeaderComponent, FooterComponent],
  templateUrl: './service-listing.component.html',
  styleUrl: './service-listing.component.scss'
})
export class ServiceListingComponent implements OnInit {
  serviceType = signal<string>('');
  location = signal<string>('Bangalore');

  // Filters
  serviceTypeFilter = signal<string>('');
  photographyStyle = signal<{ [key: string]: boolean }>({
    candid: true,
    traditional: true,
    cinematic: false,
    drone: false
  });
  experience = signal<{ [key: string]: boolean }>({
    '5plus': false,
    '3plus': false
  });
  rating = signal<{ [key: string]: boolean }>({
    '4.5': false,
    '4.0': false
  });

  vendors = signal<ServiceVendor[]>([]);
  filteredVendors = computed(() => {
    let result = [...this.vendors()];

    // Filter by photography style
    const selectedStyles = Object.entries(this.photographyStyle())
      .filter(([_, selected]) => selected)
      .map(([key, _]) => key);
    
    // Filter by experience
    const selectedExperience = Object.entries(this.experience())
      .filter(([_, selected]) => selected)
      .map(([key, _]) => key);

    // Filter by rating
    const selectedRatings = Object.entries(this.rating())
      .filter(([_, selected]) => selected)
      .map(([key, _]) => key);

    // Apply filters (simplified - in real app, this would filter based on vendor data)
    // For now, just return all vendors sorted by popularity (review count)
    result.sort((a, b) => b.reviewCount - a.reviewCount);

    return result;
  });

  constructor(
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    window.scrollTo(0, 0);
    // Get service type from route
    this.route.paramMap.subscribe(params => {
      const type = params.get('type') || 'photography';
      this.serviceType.set(type);
      this.serviceTypeFilter.set(this.formatServiceType(type));
      this.loadVendors(type);
    });
  }

  formatServiceType(type: string): string {
    return type.charAt(0).toUpperCase() + type.slice(1);
  }

  private loadVendors(serviceType: string): void {
    // TODO: Call API to fetch service vendors
    // API endpoint: GET /api/services/{type}?location={loc}&page={page}&pageSize={pageSize}
    // For now, set empty array - API integration pending
    this.vendors.set([]);
  }

  togglePhotographyStyle(style: string): void {
    this.photographyStyle.update(styles => ({
      ...styles,
      [style]: !styles[style]
    }));
  }

  toggleExperience(exp: string): void {
    this.experience.update(exps => ({
      ...exps,
      [exp]: !exps[exp]
    }));
  }

  toggleRating(rating: string): void {
    this.rating.update(ratings => ({
      ...ratings,
      [rating]: !ratings[rating]
    }));
  }

  onResetFilters(): void {
    this.photographyStyle.set({
      candid: false,
      traditional: false,
      cinematic: false,
      drone: false
    });
    this.experience.set({
      '5plus': false,
      '3plus': false
    });
    this.rating.set({
      '4.5': false,
      '4.0': false
    });
  }

  formatPrice(amount: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR',
      maximumFractionDigits: 0
    }).format(amount);
  }

  onViewVendor(vendorId: string): void {
    this.router.navigate(['/services', this.serviceType(), vendorId]);
  }

  onContactVendor(vendorId: string): void {
    // TODO: Open contact modal or navigate to contact page
    console.log('Contact vendor:', vendorId);
  }

      onLoadMore(): void {
        // TODO: Load more vendors
        console.log('Load more vendors');
      }

      goBack(): void {
        this.router.navigate(['/services']);
      }
}

