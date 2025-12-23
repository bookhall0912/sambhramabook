import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from '../../../components/header/header.component';
import { FooterComponent } from '../../../components/footer/footer.component';
import { AuthService } from '../../../services/auth.service';

export interface ServicePackage {
  id: string;
  name: string;
  price: number;
  priceUnit: string;
  features: string[];
  isPopular?: boolean;
}

export interface SimilarVendor {
  id: string;
  name: string;
  image?: string;
  price: number;
  priceUnit: string;
}

@Component({
  selector: 'app-service-detail',
  imports: [CommonModule, FormsModule, HeaderComponent, FooterComponent],
  templateUrl: './service-detail.component.html',
  styleUrl: './service-detail.component.scss'
})
export class ServiceDetailComponent implements OnInit {
  vendorId = signal<string>('');
  serviceType = signal<string>('');
  
  // Vendor data
  vendorName = signal<string>('Lens & Light Studios');
  vendorImage = signal<string>('https://images.unsplash.com/photo-1528605248644-14dd04022da1');
  vendorLocation = signal<string>('Indiranagar, Bangalore');
  vendorRating = signal<number>(4.9);
  vendorReviewCount = signal<number>(120);
  vendorExperience = signal<number>(5);
  vendorTag = signal<string>('Photography');
  isVerified = signal<boolean>(true);

  about = signal<string>('Lens & Light Studios is a premier wedding photography team based in Indiranagar, Bangalore. With over 5 years of experience, we specialize in candid photography, cinematic wedding films, and traditional coverage.');
  
  specialities = signal<string[]>([
    'Candid Photography',
    'Cinematic Video',
    'Pre-Wedding Shoots',
    'Drone Coverage'
  ]);

  packages = signal<ServicePackage[]>([
    {
      id: '1',
      name: 'Standard Package',
      price: 50000,
      priceUnit: '/ day',
      features: [
        '1 Candid Photographer',
        '1 Traditional Photographer',
        'Soft copies of images'
      ]
    },
    {
      id: '2',
      name: 'Premium Wedding',
      price: 85000,
      priceUnit: '/ day',
      features: [
        '2 Candid Photographers',
        '1 Cinematographer',
        'Drone Shoots Included'
      ],
      isPopular: true
    }
  ]);

  portfolioImages = signal<string[]>([
    'https://images.unsplash.com/photo-1511795409834-ef04bbd61622',
    'https://images.unsplash.com/photo-1528605248644-14dd04022da1',
    'https://images.unsplash.com/photo-1500530855697-b586d89ba3ee'
  ]);

  similarVendors = signal<SimilarVendor[]>([
    {
      id: '2',
      name: 'Memories by Rahul',
      image: 'https://images.unsplash.com/photo-1500530855697-b586d89ba3ee',
      price: 35000,
      priceUnit: '/ day'
    },
    {
      id: '3',
      name: 'Golden Frame',
      price: 80000,
      priceUnit: '/ day'
    },
    {
      id: '4',
      name: 'Vivid Captures',
      price: 65000,
      priceUnit: '/ day'
    },
    {
      id: '5',
      name: 'Urban Tales',
      image: 'https://images.unsplash.com/photo-1519183071298-a2962be96c66',
      price: 40000,
      priceUnit: '/ day'
    }
  ]);

  activeTab = signal<string>('overview');
  selectedPackage = signal<string>('2');
  eventDate = signal<string>('24 Oct 2024 - 25 Oct 2024');
  totalEstimate = signal<number>(200600);

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    window.scrollTo(0, 0);
    // Get vendor ID and service type from route
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      const type = params.get('type');
      if (id) {
        this.vendorId.set(id);
        this.loadVendorData(id);
      }
      if (type) {
        this.serviceType.set(type);
      }
    });
  }

  private loadVendorData(id: string): void {
    // TODO: Load vendor data from API
    // TODO: Call API to fetch vendor details
    // API endpoint: GET /api/services/{type}/{id}
    console.log('Loading vendor data for:', id);
  }

  setActiveTab(tab: string): void {
    this.activeTab.set(tab);
  }

  selectPackage(packageId: string): void {
    this.selectedPackage.set(packageId);
    this.updateTotalEstimate();
  }

  private updateTotalEstimate(): void {
    const selected = this.packages().find(p => p.id === this.selectedPackage());
    if (selected) {
      // Simple calculation - in real app, this would consider dates, add-ons, etc.
      this.totalEstimate.set(selected.price * 2); // Assuming 2 days
    }
  }

  formatPrice(amount: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR',
      maximumFractionDigits: 0
    }).format(amount);
  }

  onRequestToBook(): void {
    // Check if user is authenticated
    if (!this.authService.isAuthenticated$()) {
      // Build returnUrl with service booking details
      // Using 'service' as the ID to indicate it's a service booking
      const returnUrl = `/booking/service?vendorId=${this.vendorId()}&serviceType=${this.serviceType()}&packageId=${this.selectedPackage()}`;
      
      // Redirect to login with returnUrl
      this.router.navigate(['/login'], {
        queryParams: { returnUrl }
      });
      return;
    }

    // User is authenticated, proceed to booking
    // Using 'service' as the ID to indicate it's a service booking
    this.router.navigate(['/booking/service'], {
      queryParams: {
        vendorId: this.vendorId(),
        serviceType: this.serviceType(),
        packageId: this.selectedPackage()
      }
    });
  }

  onContactVendor(): void {
    // TODO: Open contact modal or navigate to contact page
    console.log('Contact vendor:', this.vendorId());
  }

  onShare(): void {
    // TODO: Implement share functionality
    console.log('Share vendor');
  }

  onSave(): void {
    // TODO: Implement save functionality
    console.log('Save vendor');
  }

  onViewSimilarVendor(vendorId: string): void {
    this.router.navigate(['/services', this.serviceType(), vendorId]);
  }
}


