import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { HallsListResponseDto } from '../models/dtos/halls-list-response.dto';
import { HallDetailResponseDto } from '../models/dtos/hall-detail-response.dto';
import { HallsSearchRequestDto } from '../models/dtos/halls-search-request.dto';
import { BookingRequestDto } from '../models/dtos/booking-request.dto';
import { BookingResponseDto } from '../models/dtos/booking-response.dto';
import { ServiceGetResponseDto } from '../models/dtos/service-get-response.dto';

@Injectable({ providedIn: 'root' })
export class HallsApiService {
  private readonly http = inject(HttpClient);

  /**
   * Get popular/featured halls for landing page using location-based services endpoint
   * Returns an array of ServiceGetResponseDto directly
   * Supported params: Latitude, Longitude, Limit, Radius, ServiceType
   */
  public getPopularHalls(
    longitude: number,
    latitude: number,
    limit: number = 3,
    radius: number = 50
  ): Observable<ServiceGetResponseDto[]> {
    const params = new HttpParams()
      .set('Longitude', longitude.toString())
      .set('Lattitude', latitude.toString())
      .set('ServiceType', '1') // 1 = Hall
      .set('Limit', limit.toString())
      .set('Radius', radius.toString());
    
    return this.http.get<ServiceGetResponseDto[]>(`${environment.apiUrl}/services`, { params });
  }

  /**
   * Search/filter halls using location-based services endpoint
   * Only supports: Latitude, Longitude, Limit, Radius, ServiceType, guestCount, date, days
   */
  public searchHalls(
    longitude: number,
    latitude: number,
    request: {
      limit?: number;
      radius?: number;
      guestCount?: number;
      date?: string;
      days?: number;
    }
  ): Observable<ServiceGetResponseDto[]> {
    let params = new HttpParams()
      .set('Longitude', longitude.toString())
      .set('Lattitude', latitude.toString())
      .set('ServiceType', '1') // 1 = Hall
      .set('Limit', (request.limit || 12).toString())
      .set('Radius', (request.radius || 50).toString());
    
    // Add supported optional parameters
    if (request.guestCount !== undefined) {
      params = params.set('guestCount', request.guestCount.toString());
    }
    if (request.date) {
      params = params.set('date', request.date);
    }
    if (request.days !== undefined) {
      params = params.set('days', request.days.toString());
    }

    return this.http.get<ServiceGetResponseDto[]>(`${environment.apiUrl}/services`, { params });
  }

  /**
   * Get hall details by ID
   */
  public getHallDetail(id: string): Observable<HallDetailResponseDto> {
    return this.http.get<HallDetailResponseDto>(`${environment.apiUrl}/halls/${id}`);
  }

  /**
   * Get similar halls (recommendations)
   */
  public getSimilarHalls(hallId: string, limit: number = 4): Observable<HallsListResponseDto> {
    const params = new HttpParams().set('limit', limit.toString());
    return this.http.get<HallsListResponseDto>(`${environment.apiUrl}/halls/${hallId}/similar`, { params });
  }

  /**
   * Get hall availability calendar
   */
  public getHallAvailability(hallId: string, month: string, year: number): Observable<HallAvailabilityResponseDto> {
    const params = new HttpParams()
      .set('month', month)
      .set('year', year.toString());
    return this.http.get<HallAvailabilityResponseDto>(`${environment.apiUrl}/halls/${hallId}/availability`, { params });
  }

  /**
   * Submit booking request
   */
  public submitBooking(request: BookingRequestDto): Observable<BookingResponseDto> {
    return this.http.post<BookingResponseDto>(`${environment.apiUrl}/bookings`, request);
  }
}

export interface HallAvailabilityResponseDto {
  month: string;
  year: number;
  days: CalendarDayDto[];
}

export interface CalendarDayDto {
  day: number;
  status: 'available' | 'booked' | 'unavailable';
  price?: number;
}

