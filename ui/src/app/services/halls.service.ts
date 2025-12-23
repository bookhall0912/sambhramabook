import { Injectable, signal, computed, inject } from '@angular/core';
import { HallCardData } from '../components/venue-card/venue-card.component';
import { HallDetail } from '../pages/halls/hall-detail/hall-detail.component';
import { HallDto } from '../models/dtos/hall.dto';
import { HallDetailResponseDto } from '../models/dtos/hall-detail-response.dto';
import { Review } from '../components/reviews-section/reviews-section.component';
import { CalendarDay } from '../components/availability-calendar/availability-calendar.component';
import { HallsApiService, HallAvailabilityResponseDto } from './halls.api';
import { ServiceGetResponseDto } from '../models/dtos/service-get-response.dto';
import { catchError, of } from 'rxjs';

interface HallsListState {
  halls: HallCardData[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
  loading: boolean;
  error: string | null;
}

interface HallDetailState {
  hall: HallDetail | null;
  loading: boolean;
  error: string | null;
}

@Injectable({ providedIn: 'root' })
export class HallsService {
  private readonly hallsApiService = inject(HallsApiService);

  // Store user's location (default to Bangalore)
  private readonly userLocation = signal<{ latitude: number; longitude: number }>({
    latitude: 12.9716,
    longitude: 77.5946
  });

  // Popular halls state
  private readonly popularHallsState = signal<HallCardData[]>([]);
  public readonly popularHalls$ = computed(() => this.popularHallsState());

  // Halls list state
  private readonly hallsListState = signal<HallsListState>({
    halls: [],
    total: 0,
    page: 1,
    pageSize: 12,
    totalPages: 0,
    loading: false,
    error: null
  });
  public readonly hallsList$ = computed(() => this.hallsListState());

  // Hall detail state
  private readonly hallDetailState = signal<HallDetailState>({
    hall: null,
    loading: false,
    error: null
  });
  public readonly hallDetail$ = computed(() => this.hallDetailState());

  /**
   * Fetch popular halls for landing page from API
   */
  public fetchPopularHalls(limit: number = 3, radius: number = 50): void {
    const location = this.userLocation();
    this.hallsApiService.getPopularHalls(location.longitude, location.latitude, limit, radius)
      .pipe(
        catchError(error => {
          console.error('Error fetching popular halls:', error);
          return of([]);
        })
      )
      .subscribe(halls => {
        const cardData = this.convertServiceResponseToCardData(halls);
        this.popularHallsState.set(cardData);
      });
  }

  /**
   * Calculate distance between two coordinates using Haversine formula
   */
  private calculateDistance(lat1: number, lon1: number, lat2: number, lon2: number): number {
    const R = 6371; // Radius of the Earth in km
    const dLat = this.toRad(lat2 - lat1);
    const dLon = this.toRad(lon2 - lon1);
    const a = 
      Math.sin(dLat / 2) * Math.sin(dLat / 2) +
      Math.cos(this.toRad(lat1)) * Math.cos(this.toRad(lat2)) *
      Math.sin(dLon / 2) * Math.sin(dLon / 2);
    const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    return R * c;
  }

  private toRad(degrees: number): number {
    return degrees * (Math.PI / 180);
  }


  /**
   * Search halls with filters from API
   */
  public searchHalls(request: any, radius: number = 50): void {
    this.hallsListState.update(state => ({ ...state, loading: true, error: null }));

    const location = this.userLocation();
    this.hallsApiService.searchHalls(location.longitude, location.latitude, {
      limit: request.pageSize || 12,
      radius: radius,
      guestCount: request.guests,
      date: request.date,
      days: request.days
    })
      .pipe(
        catchError(error => {
          console.error('Error searching halls:', error);
          this.hallsListState.update(state => ({
            ...state,
            loading: false,
            error: 'Failed to search halls. Please try again.'
          }));
          return of([]);
        })
      )
      .subscribe(halls => {
        const cardData = this.convertServiceResponseToCardData(halls);
        const pageSize = request.pageSize || 12;
        const page = request.page || 1;
        
        this.hallsListState.set({
          halls: cardData,
          total: cardData.length,
          page: page,
          pageSize: pageSize,
          totalPages: Math.ceil(cardData.length / pageSize),
          loading: false,
          error: null
        });
      });
  }

  /**
   * Fetch hall detail by ID from API
   */
  public fetchHallDetail(id: string): void {
    this.hallDetailState.update(state => ({ ...state, loading: true, error: null }));

    this.hallsApiService.getHallDetail(id)
      .pipe(
        catchError(error => {
          console.error('Error fetching hall detail:', error);
          this.hallDetailState.update(state => ({
            ...state,
            loading: false,
            error: 'Failed to load hall details. Please try again.'
          }));
          return of(null);
        })
      )
      .subscribe(detail => {
        if (detail) {
          this.hallDetailState.set({
            hall: this.convertToHallDetail(detail),
            loading: false,
            error: null
          });
        } else {
          this.hallDetailState.update(state => ({
            ...state,
            loading: false,
            error: 'Hall not found'
          }));
        }
      });
  }

  /**
   * Fetch similar halls from API
   */
  public fetchSimilarHalls(hallId: string, limit: number = 4): Promise<HallCardData[]> {
    return new Promise((resolve, reject) => {
      this.hallsApiService.getSimilarHalls(hallId, limit)
        .pipe(
          catchError(error => {
            console.error('Error fetching similar halls:', error);
            return of({ halls: [], total: 0, page: 1, pageSize: limit, totalPages: 0 });
          })
        )
        .subscribe(response => {
          const cardData = this.convertHallsToCardData(response.halls);
          resolve(cardData);
        });
    });
  }

  /**
   * Fetch hall availability from API
   */
  public fetchHallAvailability(hallId: string, month: string, year: number): Promise<CalendarDay[]> {
    return new Promise((resolve, reject) => {
      this.hallsApiService.getHallAvailability(hallId, month, year)
        .pipe(
          catchError(error => {
            console.error('Error fetching hall availability:', error);
            // Return empty calendar on error
            return of({ month, year, days: [] });
          })
        )
        .subscribe(response => {
          const calendarDays = this.convertToCalendarDays(response.days);
          resolve(calendarDays);
        });
    });
  }

  /**
   * Submit booking to API
   */
  public submitBooking(request: any): Promise<any> {
    return new Promise((resolve, reject) => {
      this.hallsApiService.submitBooking(request)
        .pipe(
          catchError(error => {
            console.error('Error submitting booking:', error);
            reject(error);
            return of(null);
          })
        )
        .subscribe(response => {
          if (response) {
            resolve(response);
          } else {
            reject(new Error('Failed to submit booking'));
          }
        });
    });
  }

  // Converters
  private convertHallsToCardData(halls: HallDto[]): HallCardData[] {
    return halls.map(hall => ({
      id: hall.id,
      name: hall.name,
      location: hall.location,
      distance: hall.distance,
      rating: hall.rating,
      capacity: hall.capacity,
      rooms: hall.rooms,
      price: hall.price,
      imageUrl: hall.imageUrl,
      amenities: hall.amenities
    }));
  }

  /**
   * Convert ServiceGetResponseDto to HallCardData
   */
  private convertServiceResponseToCardData(services: ServiceGetResponseDto[]): HallCardData[] {
    return services.map(service => ({
      id: service.id,
      name: service.title,
      location: service.location || service.city,
      distance: this.calculateDistanceString(service.latitude, service.longitude),
      rating: service.rating || 0,
      capacity: service.capacity || service.maxCapacity || 0,
      rooms: service.rooms || 0,
      price: service.price || 0,
      imageUrl: service.imageUrl || service.images?.[0] || '',
      amenities: service.amenities || []
    }));
  }

  /**
   * Calculate distance string from user location
   */
  private calculateDistanceString(lat?: number, lon?: number): string | undefined {
    if (!lat || !lon) return undefined;
    const location = this.userLocation();
    const distance = this.calculateDistance(location.latitude, location.longitude, lat, lon);
    return `${distance.toFixed(1)} km`;
  }

  private convertToHallDetail(dto: HallDetailResponseDto): HallDetail {
    return {
      id: dto.id,
      name: dto.name,
      location: dto.location,
      distance: dto.distance,
      rating: dto.rating,
      capacity: dto.capacity,
      minCapacity: dto.minCapacity,
      maxCapacity: dto.maxCapacity,
      rooms: dto.rooms,
      price: dto.price,
      imageUrl: dto.imageUrl,
      images: dto.images || [dto.imageUrl],
      amenities: dto.amenities,
      description: dto.fullDescription || dto.description,
      fullAmenities: dto.fullAmenities || dto.amenities,
      parking: dto.parking,
      reviews: this.convertToReviews(dto.reviews)
    };
  }

  private convertToReviews(reviews: any[]): Review[] {
    return reviews.map(review => ({
      id: review.id,
      author: review.author,
      rating: review.rating,
      comment: review.comment,
      date: review.date
    }));
  }

  private convertToCalendarDays(days: any[]): CalendarDay[] {
    return days.map(day => ({
      day: day.day,
      status: day.status === 'booked' ? 'booked' : day.status === 'unavailable' ? 'booked' : 'available'
    }));
  }
}

