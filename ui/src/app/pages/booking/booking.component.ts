import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HeaderComponent } from '../../components/header/header.component';
import { FooterComponent } from '../../components/footer/footer.component';
import { StepperComponent } from '../../components/stepper/stepper.component';
import { AddonCardComponent } from '../../components/addon-card/addon-card.component';
import { BookingSummaryComponent } from '../../components/booking-summary/booking-summary.component';
import { AuthService } from '../../services/auth.service';

export interface Addon {
  id: string;
  title: string;
  description: string;
  price: number;
  selected: boolean;
}

export interface BookingData {
  hallId: string;
  hallName: string;
  hallImage: string;
  location: string;
  date: string;
  days: number;
  guests: number;
  basePrice: number;
  addons: Addon[];
  specialRequirements?: string;
}

@Component({
  selector: 'app-booking',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    HeaderComponent,
    FooterComponent,
    StepperComponent,
    AddonCardComponent,
    BookingSummaryComponent
  ],
  templateUrl: './booking.component.html',
  styleUrl: './booking.component.scss'
})
export class BookingComponent implements OnInit {
  currentStep = signal<number>(2); // Step 2 (Add-ons) is active by default, Step 3 is Review & Confirm, Step 4 is Payment
  bookingData = signal<BookingData | null>(null);
  isProcessingPayment = signal<boolean>(false);
  paymentMethod = signal<string>('UPI');
  
  addonsForm: FormGroup;
  addons = signal<Addon[]>([
    {
      id: '1',
      title: 'Drone Coverage (Extra Hours)',
      description: 'Extended aerial shots (2 hrs)',
      price: 15000,
      selected: true
    },
    {
      id: '2',
      title: 'Same Day Edit (Video Teaser)',
      description: '3-min highlight video',
      price: 25000,
      selected: false
    },
    {
      id: '3',
      title: 'Hardcover Album Upgrade',
      description: 'Italian leather finish',
      price: 12000,
      selected: true
    },
    {
      id: '4',
      title: 'Photo Booth with Props',
      description: 'Instant guest prints',
      price: 18000,
      selected: false
    }
  ]);

  // Computed property for selected addons
  selectedAddons = computed(() => {
    return this.addons().filter(a => a.selected);
  });

  totalPrice = computed(() => {
    const data = this.bookingData();
    if (!data) return 0;
    
    const basePrice = data.basePrice || 170000;
    const addonsTotal = this.selectedAddons()
      .reduce((sum, a) => sum + a.price, 0);
    const subtotal = basePrice + addonsTotal;
    const platformFee = subtotal * 0.02;
    const taxes = subtotal * 0.18;
    
    return subtotal + platformFee + taxes;
  });

  priceBreakdown = computed(() => {
    const data = this.bookingData();
    if (!data) {
      return {
        basePrice: 0,
        addonsTotal: 0,
        platformFee: 0,
        taxes: 0,
        total: 0
      };
    }
    
    const basePrice = data.basePrice || 170000;
    const addonsTotal = this.selectedAddons()
      .reduce((sum, a) => sum + a.price, 0);
    const subtotal = basePrice + addonsTotal;
    const platformFee = subtotal * 0.02;
    const taxes = subtotal * 0.18;
    const total = subtotal + platformFee + taxes;
    
    return {
      basePrice,
      addonsTotal,
      platformFee,
      taxes,
      total
    };
  });

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ) {
    this.addonsForm = this.fb.group({
      specialRequirements: ['']
    });
  }

  ngOnInit(): void {
    // Scroll to top when component initializes
    window.scrollTo(0, 0);
    
    // Check if user is authenticated
    if (!this.authService.isAuthenticated$()) {
      // Build returnUrl with current route and query params
      const currentUrl = this.router.url;
      this.router.navigate(['/login'], {
        queryParams: { returnUrl: currentUrl }
      });
      return;
    }
    
    // Get booking data from route params and query params
    this.route.params.subscribe(params => {
      const bookingId = params['id'];
      
      // Get query params for booking details
      this.route.queryParams.subscribe(queryParams => {
        // Check if this is a service booking (vendorId in query params)
        const vendorId = queryParams['vendorId'];
        const serviceType = queryParams['serviceType'];
        const packageId = queryParams['packageId'];
        
        if (vendorId && serviceType) {
          // Service booking
          // TODO: Fetch service vendor details from service using vendorId
          // For now, using mock data
          const selectedPackage = this.getServicePackagePrice(packageId);
          this.bookingData.set({
            hallId: vendorId, // Reusing hallId field for vendorId
            hallName: 'Lens & Light Studios', // TODO: Get from service
            hallImage: 'https://images.unsplash.com/photo-1528605248644-14dd04022da1',
            location: 'Bangalore',
            date: queryParams['date'] || '2024-10-24',
            days: parseInt(queryParams['days'] || '1', 10),
            guests: parseInt(queryParams['guests'] || '500', 10),
            basePrice: selectedPackage,
            addons: this.addons()
          });
        } else {
          // Hall booking
          const date = queryParams['date'] || '2024-10-24';
          const days = parseInt(queryParams['days'] || '2', 10);
          const guests = parseInt(queryParams['guests'] || '500', 10);
          
          // TODO: Fetch hall details from service using hallId
          // For now, using mock data
          this.bookingData.set({
            hallId: bookingId || '1',
            hallName: 'Lens & Light Studios',
            hallImage: 'https://images.unsplash.com/photo-1519741497674-611481863552?w=800&h=600&fit=crop',
            location: 'Bangalore',
            date: date,
            days: days,
            guests: guests,
            basePrice: 170000,
            addons: this.addons()
          });
        }
      });
    });
  }

  private getServicePackagePrice(packageId: string | null): number {
    // Mock package prices - in real app, fetch from service
    const packagePrices: { [key: string]: number } = {
      '1': 50000, // Standard Package
      '2': 85000  // Premium Wedding
    };
    return packagePrices[packageId || '2'] || 85000;
  }

  toggleAddon(addon: Addon): void {
    this.addons.update(addons => 
      addons.map(a => a.id === addon.id ? { ...a, selected: !a.selected } : a)
    );
  }

  goToStep(step: number): void {
    if (step <= this.currentStep()) {
      this.currentStep.set(step);
    }
  }

  goBack(): void {
    if (this.currentStep() > 1) {
      this.currentStep.update(step => step - 1);
    } else {
      // Navigate back to hall detail
      const data = this.bookingData();
      if (data) {
        this.router.navigate(['/halls', data.hallId]);
      }
    }
  }

  continueToNext(): void {
    if (this.currentStep() < 4) {
      this.currentStep.update(step => step + 1);
    } else {
      // Step 4: Process payment
      this.processPayment();
    }
  }

  formatDateRange(date: string, days: number): string {
    const startDate = new Date(date);
    const endDate = new Date(startDate);
    endDate.setDate(startDate.getDate() + days - 1);
    
    const formatDate = (d: Date) => {
      const day = d.getDate();
      const month = d.toLocaleString('en-US', { month: 'short' });
      const year = d.getFullYear();
      return `${day} ${month} ${year}`;
    };
    
    if (days === 1) {
      return formatDate(startDate);
    }
    
    return `${formatDate(startDate)} – ${formatDate(endDate)}`;
  }

  formatPrice(price: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR',
      maximumFractionDigits: 0
    }).format(price);
  }

  private processPayment(): void {
    const data = this.bookingData();
    if (!data) {
      console.error('No booking data available');
      return;
    }

    // For testing: immediately redirect to confirmation
    // Generate transaction ID
    const transactionId = Math.floor(Math.random() * 100000000000).toString();
    
    // Generate booking ID
    const mockBookingId = 'SB-' + Math.floor(Math.random() * 10000) + '-X' + Math.floor(Math.random() * 1000);
    
    // Navigate to confirmation page with booking details
    const queryParams = new URLSearchParams({
      bookingId: mockBookingId,
      transactionId: transactionId,
      paymentMethod: this.paymentMethod(),
      totalAmount: this.totalPrice().toString()
    });
    
    const url = `/booking/confirmation?${queryParams.toString()}`;
    console.log('Navigating to:', url);
    
    this.router.navigateByUrl(url).then(success => {
      if (success) {
        console.log('Navigation successful');
      } else {
        console.error('Navigation failed - trying alternative route');
        // Fallback: try with router.navigate
        this.router.navigate(['/booking/confirmation'], {
          queryParams: {
            bookingId: mockBookingId,
            transactionId: transactionId,
            paymentMethod: this.paymentMethod(),
            totalAmount: this.totalPrice().toString()
          }
        });
      }
    }).catch(error => {
      console.error('Navigation error:', error);
    });
  }
}

