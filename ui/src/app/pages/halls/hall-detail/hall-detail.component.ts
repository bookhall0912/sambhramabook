import { Component, OnInit, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { HeaderComponent } from '../../../components/header/header.component';
import { FooterComponent } from '../../../components/footer/footer.component';
import { HallCardData } from '../../../components/venue-card/venue-card.component';
import { ImageGalleryComponent } from '../../../components/image-gallery/image-gallery.component';
import { BookingCardComponent, BookingData } from '../../../components/booking-card/booking-card.component';
import { ReviewsSectionComponent, Review } from '../../../components/reviews-section/reviews-section.component';
import { AvailabilityCalendarComponent, CalendarDay } from '../../../components/availability-calendar/availability-calendar.component';
import { TabsComponent, Tab } from '../../../components/tabs/tabs.component';
import { HallsService } from '../../../services/halls.service';
import { AuthService } from '../../../services/auth.service';
import { Venue } from '../../../models/venue.model';

export interface HallDetail extends Venue {
  distance?: string;
  amenities?: string[];
  images?: string[];
  description?: string;
  fullAmenities?: string[];
  parking?: string;
  reviews?: Review[];
  minCapacity?: number;
  maxCapacity?: number;
}

@Component({
  selector: 'app-hall-detail',
  imports: [
    CommonModule,
    HeaderComponent,
    FooterComponent,
    RouterLink,
    ImageGalleryComponent,
    BookingCardComponent,
    ReviewsSectionComponent,
    AvailabilityCalendarComponent,
    TabsComponent
  ],
  templateUrl: './hall-detail.component.html',
  styleUrl: './hall-detail.component.scss'
})
export class HallDetailComponent implements OnInit {
  hall: HallDetail | null = null;
  activeTab = 'overview';
  similarHalls: HallCardData[] = [];
  calendarDays: CalendarDay[] = [];
  selectedDay: number | null = null;
  tabs: Tab[] = [
    { id: 'overview', label: 'Overview' },
    { id: 'amenities', label: 'Amenities' },
    { id: 'pricing', label: 'Pricing' },
    { id: 'availability', label: 'Availability' },
    { id: 'reviews', label: 'Reviews' }
  ];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private hallsService: HallsService,
    private authService: AuthService
  ) {
    effect(() => {
      const state = this.hallsService.hallDetail$();
      if (state && state.hall) {
        this.hall = state.hall;
      }
    });
  }

  ngOnInit(): void {
    const hallId = this.route.snapshot.paramMap.get('id');
    if (hallId) {
      this.loadHallDetail(hallId);
      this.loadSimilarHalls(hallId);
      this.loadAvailability(hallId);
    }
  }

  loadHallDetail(id: string): void {
    this.hallsService.fetchHallDetail(id);
  }

  loadAvailability(hallId: string): void {
    const now = new Date();
    const month = now.toLocaleString('default', { month: 'long' });
    const year = now.getFullYear();
    
    this.hallsService.fetchHallAvailability(hallId, month, year)
      .then(days => {
        this.calendarDays = days;
      })
      .catch(error => {
        console.error('Error loading availability:', error);
        // Fallback to empty calendar
        this.calendarDays = [];
      });
  }

  loadSimilarHalls(excludeId: string): void {
    this.hallsService.fetchSimilarHalls(excludeId, 4)
      .then(halls => {
        this.similarHalls = halls;
      })
      .catch(error => {
        console.error('Error loading similar halls:', error);
        this.similarHalls = [];
      });
  }

  setActiveTab(tab: string): void {
    this.activeTab = tab;
  }

  formatPrice(price: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR',
      maximumFractionDigits: 0
    }).format(price);
  }

  onBookingSubmit(bookingData: BookingData): void {
    if (!this.hall) return;

    // Check if user is authenticated
    if (!this.authService.isAuthenticated$()) {
      // Build returnUrl with booking details
      const returnUrl = `/booking/${this.hall.id}?date=${encodeURIComponent(bookingData.date)}&days=${bookingData.days}&guests=${bookingData.guests}`;
      
      // Redirect to login with returnUrl
      this.router.navigate(['/login'], {
        queryParams: { returnUrl }
      });
      return;
    }

    // User is authenticated, proceed to booking
    this.router.navigate(['/booking', this.hall.id], {
      queryParams: {
        date: bookingData.date,
        days: bookingData.days,
        guests: bookingData.guests
      }
    });
  }

  onDaySelected(day: number): void {
    console.log('Day selected:', day);
    this.selectedDay = day;
    // Update calendar days to mark selected day
    this.calendarDays = this.calendarDays.map(d => ({
      ...d,
      status: d.day === day ? 'selected' : d.status === 'selected' ? 'available' : d.status
    }));
  }

  getReviewCount(): number {
    return this.hall?.reviews?.length || 128;
  }
}



