import { Injectable, signal, computed } from '@angular/core';
import { HallCardData } from '../components/venue-card/venue-card.component';
import { HallDetail } from '../pages/halls/hall-detail/hall-detail.component';
import { HallDto } from '../models/dtos/hall.dto';
import { HallDetailResponseDto } from '../models/dtos/hall-detail-response.dto';
import { Review } from '../components/reviews-section/reviews-section.component';
import { CalendarDay } from '../components/availability-calendar/availability-calendar.component';
import { MOCK_HALLS, MOCK_HALL_DETAILS, MOCK_SIMILAR_HALLS } from '../data/mock-halls.data';

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
   * Fetch popular halls for landing page using mock data
   */
  public fetchPopularHalls(limit: number = 3, radius: number = 50): void {
    // Use mock data directly
    this.popularHallsState.set(this.convertHallsToCardData(MOCK_HALLS.slice(0, limit)));
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
   * Search halls with filters using mock data
   */
  public searchHalls(request: any, radius: number = 50): void {
    this.hallsListState.update(state => ({ ...state, loading: true, error: null }));

    // Use mock data directly
    const filteredHalls = this.filterMockHalls(request);
    const pageSize = request.pageSize || 12;
    const page = request.page || 1;
    const startIndex = (page - 1) * pageSize;
    const endIndex = startIndex + pageSize;
    const paginatedHalls = filteredHalls.slice(startIndex, endIndex);

    this.hallsListState.set({
      halls: this.convertHallsToCardData(paginatedHalls),
      total: filteredHalls.length,
      page: page,
      pageSize: pageSize,
      totalPages: Math.ceil(filteredHalls.length / pageSize),
      loading: false,
      error: null
    });
  }

  /**
   * Fetch hall detail by ID using mock data
   */
  public fetchHallDetail(id: string): void {
    this.hallDetailState.update(state => ({ ...state, loading: true, error: null }));

    // Use mock data directly
    const mockDetail = MOCK_HALL_DETAILS[id] || this.createMockDetailFromHall(id);
    if (mockDetail) {
      this.hallDetailState.set({
        hall: this.convertToHallDetail(mockDetail),
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
  }

  /**
   * Fetch similar halls using mock data
   */
  public fetchSimilarHalls(hallId: string, limit: number = 4): Promise<HallCardData[]> {
    return new Promise((resolve) => {
      // Use mock data directly
      const mockHalls = MOCK_SIMILAR_HALLS[hallId] || MOCK_HALLS.filter(h => h.id !== hallId).slice(0, limit);
      resolve(this.convertHallsToCardData(mockHalls));
    });
  }

  /**
   * Fetch hall availability using mock data
   */
  public fetchHallAvailability(hallId: string, month: string, year: number): Promise<CalendarDay[]> {
    return new Promise((resolve) => {
      // Generate mock calendar days
      const days: CalendarDay[] = [];
      const daysInMonth = new Date(year, parseInt(month) - 1, 0).getDate();
      for (let day = 1; day <= daysInMonth; day++) {
        // Randomly mark some days as booked
        const status = Math.random() > 0.7 ? 'booked' : 'available';
        days.push({ day, status });
      }
      resolve(days);
    });
  }

  /**
   * Submit booking using mock data
   */
  public submitBooking(request: any): Promise<any> {
    return new Promise((resolve) => {
      // Simulate API delay
      setTimeout(() => {
        resolve({
          bookingId: `BK-${Date.now()}`,
          status: 'confirmed',
          message: 'Booking submitted successfully'
        });
      }, 500);
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

  /**
   * Filter mock halls based on search criteria
   */
  private filterMockHalls(request: any): HallDto[] {
    let filtered = [...MOCK_HALLS];

    // Filter by location
    if (request.location) {
      const locationLower = request.location.toLowerCase();
      filtered = filtered.filter(hall => 
        hall.location.toLowerCase().includes(locationLower)
      );
    }

    // Filter by price range
    if (request.maxPrice !== undefined) {
      filtered = filtered.filter(hall => hall.price <= request.maxPrice);
    }
    if (request.minPrice !== undefined) {
      filtered = filtered.filter(hall => hall.price >= request.minPrice);
    }

    // Filter by capacity
    if (request.minCapacity !== undefined) {
      filtered = filtered.filter(hall => 
        (hall.maxCapacity || hall.capacity) >= request.minCapacity
      );
    }
    if (request.maxCapacity !== undefined) {
      filtered = filtered.filter(hall => 
        (hall.minCapacity || hall.capacity) <= request.maxCapacity
      );
    }

    // Filter by amenities
    if (request.amenities && request.amenities.length > 0) {
      filtered = filtered.filter(hall => {
        return request.amenities.some((amenity: string) => 
          hall.amenities.some(hallAmenity => 
            hallAmenity.toLowerCase().includes(amenity.toLowerCase())
          )
        );
      });
    }

    return filtered;
  }

  /**
   * Create mock hall detail from basic hall data
   */
  private createMockDetailFromHall(id: string): HallDetailResponseDto | null {
    const hall = MOCK_HALLS.find(h => h.id === id);
    if (!hall) return null;

    return {
      ...hall,
      fullDescription: hall.description || `Experience luxury and elegance at ${hall.name}.`,
      fullAmenities: hall.amenities.length > 0 
        ? hall.amenities.map(a => a === 'AC' ? 'Centralized Air Conditioning' : a)
        : ['Basic Amenities'],
      reviews: [
        {
          id: '1',
          author: 'Sample Reviewer',
          rating: hall.rating,
          comment: 'Great venue with excellent facilities.',
          date: new Date().toISOString().split('T')[0],
          verified: true
        }
      ],
      policies: ['Advance booking required'],
      cancellationPolicy: 'Standard cancellation policy applies',
      checkInTime: '10:00 AM',
      checkOutTime: '11:00 PM'
    };
  }
}

