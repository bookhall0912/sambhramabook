import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';

export interface VendorBookingDto {
  id: string;
  bookingId: string;
  customerName: string;
  customerPhone: string;
  venueName: string;
  date: string;
  days: number;
  guests: number;
  amount: number;
  status: 'PENDING' | 'CONFIRMED' | 'CANCELLED';
}

export interface GetVendorBookingsResponse {
  success: boolean;
  data: VendorBookingDto[];
  pagination?: {
    page: number;
    pageSize: number;
    total: number;
    totalPages: number;
  };
}

export interface VendorListingDto {
  id: string;
  name: string;
  image?: string;
  location: string;
  status: 'ACTIVE' | 'DRAFT' | 'INACTIVE';
  price?: number;
  capacity?: number;
  views?: number;
  bookings?: number;
}

export interface GetVendorListingsResponse {
  success: boolean;
  data: VendorListingDto[];
  pagination?: {
    page: number;
    pageSize: number;
    total: number;
    totalPages: number;
  };
}

export interface DashboardOverviewDto {
  totalBookings: number;
  pendingBookings: number;
  totalRevenue: number;
  activeListings: number;
  recentBookings: VendorBookingDto[];
}

export interface GetDashboardOverviewResponse {
  success: boolean;
  data: DashboardOverviewDto;
}

export interface CompleteOnboardingRequest {
  businessName: string;
  businessType: string;
  businessEmail: string;
  businessPhone: string;
  addressLine1: string;
  city: string;
  state: string;
  pincode: string;
  latitude?: number;
  longitude?: number;
}

@Injectable({ providedIn: 'root' })
export class VendorApiService {
  private readonly http = inject(HttpClient);

  /**
   * Get vendor bookings
   */
  public getVendorBookings(status: string = 'all', page: number = 1, pageSize: number = 10): Observable<GetVendorBookingsResponse> {
    const params = new HttpParams()
      .set('status', status)
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    
    return this.http.get<GetVendorBookingsResponse>(`${environment.apiUrl}/vendor/bookings`, { params });
  }

  /**
   * Approve booking
   */
  public approveBooking(bookingId: string): Observable<any> {
    return this.http.put(`${environment.apiUrl}/vendor/bookings/${bookingId}/approve`, {});
  }

  /**
   * Reject booking
   */
  public rejectBooking(bookingId: string, reason?: string): Observable<any> {
    return this.http.put(`${environment.apiUrl}/vendor/bookings/${bookingId}/reject`, { reason });
  }

  /**
   * Get vendor listings
   */
  public getVendorListings(page: number = 1, pageSize: number = 10): Observable<GetVendorListingsResponse> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    
    return this.http.get<GetVendorListingsResponse>(`${environment.apiUrl}/vendor/listings`, { params });
  }

  /**
   * Get vendor listing details
   */
  public getVendorListingDetails(id: string): Observable<any> {
    return this.http.get(`${environment.apiUrl}/vendor/listings/${id}`);
  }

  /**
   * Create vendor listing
   */
  public createVendorListing(request: any): Observable<any> {
    return this.http.post(`${environment.apiUrl}/vendor/listings`, request);
  }

  /**
   * Update vendor listing
   */
  public updateVendorListing(id: string, request: any): Observable<any> {
    return this.http.put(`${environment.apiUrl}/vendor/listings/${id}`, request);
  }

  /**
   * Delete vendor listing
   */
  public deleteVendorListing(id: string): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/vendor/listings/${id}`);
  }

  /**
   * Get dashboard overview
   */
  public getDashboardOverview(): Observable<GetDashboardOverviewResponse> {
    return this.http.get<GetDashboardOverviewResponse>(`${environment.apiUrl}/vendor/dashboard/overview`);
  }

  /**
   * Get recent bookings
   */
  public getRecentBookings(limit: number = 5): Observable<any> {
    const params = new HttpParams().set('limit', limit.toString());
    return this.http.get(`${environment.apiUrl}/vendor/dashboard/recent-bookings`, { params });
  }

  /**
   * Complete onboarding
   */
  public completeOnboarding(request: CompleteOnboardingRequest): Observable<any> {
    return this.http.post(`${environment.apiUrl}/vendor/onboarding`, request);
  }

  /**
   * Get onboarding status
   */
  public getOnboardingStatus(): Observable<any> {
    return this.http.get(`${environment.apiUrl}/vendor/onboarding/status`);
  }

  /**
   * Update onboarding
   */
  public updateOnboarding(request: CompleteOnboardingRequest): Observable<any> {
    return this.http.put(`${environment.apiUrl}/vendor/onboarding`, request);
  }
}

