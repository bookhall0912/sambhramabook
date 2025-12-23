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
import { HallsService } from '../../services/halls.service';

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
    private authService: AuthService,
    private hallsService: HallsService
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
          // For now, using default values - will be implemented when service APIs are ready
          const selectedPackage = this.getServicePackagePrice(packageId);
          this.bookingData.set({
            hallId: vendorId, // Reusing hallId field for vendorId
            hallName: 'Service Provider', // TODO: Get from service API
            hallImage: '',
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
          
          // Fetch hall details from service using hallId
          const hallId = bookingId || '1';
          this.hallsService.fetchHallDetail(hallId);
          
          // Watch for hall detail updates
          // Use setTimeout to wait for async fetch to complete
          setTimeout(() => {
            const detail = this.hallsService.hallDetail$();
            if (detail?.hall) {
              const hall = detail.hall;
              this.bookingData.set({
                hallId: hallId,
                hallName: hall.name,
                hallImage: hall.imageUrl,
                location: hall.location,
                date: date,
                days: days,
                guests: guests,
                basePrice: hall.price,
                addons: this.addons()
              });
            }
          }, 100);
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

  private async processPayment(): Promise<void> {
    const data = this.bookingData();
    if (!data) {
      console.error('No booking data available');
      return;
    }

    // For testing: immediately redirect to confirmation
    // Generate transaction ID
    const transactionId = Math.floor(Math.random() * 100000000000).toString();
    
    // Calculate end date
    const startDate = new Date(data.date);
    const endDate = new Date(startDate);
    endDate.setDate(startDate.getDate() + data.days - 1);
    const endDateStr = endDate.toISOString().split('T')[0];

    // Get user info for contact
    const user = this.authService.currentUser$();
    
    // Submit booking to API
    const bookingRequest = {
      hallId: data.hallId,
      startDate: data.date,
      endDate: endDateStr,
      numberOfDays: data.days,
      guests: data.guests,
      contactInfo: {
        name: user?.name || '',
        email: user?.email || '',
        phone: user?.mobile || ''
      },
      specialRequests: data.specialRequirements || ''
    };

    try {
      const bookingResponse = await this.hallsService.submitBooking(bookingRequest);
      
      // Navigate to confirmation page with booking details
      const queryParams = new URLSearchParams({
        bookingId: bookingResponse.bookingId || bookingResponse.confirmationNumber || '',
        transactionId: transactionId,
        paymentMethod: this.paymentMethod(),
        totalAmount: this.totalPrice().toString()
      });
    
      const url = `/booking/confirmation?${queryParams.toString()}`;
      this.router.navigateByUrl(url);
    } catch (error) {
      console.error('Error submitting booking:', error);
      // TODO: Show error message to user
      this.isProcessingPayment.set(false);
    }
  }
}

