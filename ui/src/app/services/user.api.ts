import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';

export interface UserBookingDto {
  id: string;
  referenceId: string;
  venueName: string;
  venueImage?: string;
  location: string;
  startDate: string;
  endDate: string;
  days: number;
  guestCount: number;
  totalAmount: number;
  status: 'UPCOMING' | 'PAST' | 'CANCELLED' | 'PENDING_CONFIRMATION';
  paymentStatus: 'PAID' | 'PENDING' | 'REFUNDED';
  eventType?: string;
}

export interface GetUserBookingsResponse {
  success: boolean;
  data: UserBookingDto[];
  pagination?: {
    page: number;
    pageSize: number;
    total: number;
    totalPages: number;
  };
}

export interface UserProfileDto {
  id: string;
  name: string;
  email?: string;
  mobile?: string;
  address?: string;
  city?: string;
  state?: string;
  pincode?: string;
}

export interface UpdateUserProfileRequest {
  name?: string;
  email?: string;
  address?: string;
  city?: string;
  state?: string;
  pincode?: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
}

export interface SavedListingDto {
  id: string;
  listingId: string;
  title: string;
  imageUrl?: string;
  location: string;
  rating?: number;
  reviewCount?: number;
  price?: number;
  capacity?: number;
}

export interface GetSavedListingsResponse {
  success: boolean;
  data: SavedListingDto[];
}

export interface NotificationDto {
  id: string;
  title: string;
  message: string;
  type: 'booking' | 'payment' | 'reminder' | 'system';
  read: boolean;
  timestamp: string;
  actionUrl?: string;
}

export interface GetNotificationsResponse {
  success: boolean;
  data: NotificationDto[];
  pagination?: {
    page: number;
    pageSize: number;
    total: number;
    totalPages: number;
  };
}

@Injectable({ providedIn: 'root' })
export class UserApiService {
  private readonly http = inject(HttpClient);

  /**
   * Get user bookings
   */
  public getUserBookings(status?: string, page: number = 1, pageSize: number = 10): Observable<GetUserBookingsResponse> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    
    if (status) {
      params = params.set('status', status);
    }
    
    return this.http.get<GetUserBookingsResponse>(`${environment.apiUrl}/user/bookings`, { params });
  }

  /**
   * Get user booking by ID
   */
  public getUserBooking(id: string): Observable<any> {
    return this.http.get(`${environment.apiUrl}/user/bookings/${id}`);
  }

  /**
   * Get user profile
   */
  public getUserProfile(): Observable<{ success: boolean; data: UserProfileDto }> {
    return this.http.get<{ success: boolean; data: UserProfileDto }>(`${environment.apiUrl}/user/profile`);
  }

  /**
   * Update user profile
   */
  public updateUserProfile(request: UpdateUserProfileRequest): Observable<any> {
    return this.http.put(`${environment.apiUrl}/user/profile`, request);
  }

  /**
   * Change password
   */
  public changePassword(request: ChangePasswordRequest): Observable<any> {
    return this.http.put(`${environment.apiUrl}/user/profile/password`, request);
  }

  /**
   * Get saved listings
   */
  public getSavedListings(): Observable<GetSavedListingsResponse> {
    return this.http.get<GetSavedListingsResponse>(`${environment.apiUrl}/user/saved`);
  }

  /**
   * Save a listing
   */
  public saveListing(listingId: string): Observable<any> {
    return this.http.post(`${environment.apiUrl}/user/saved/${listingId}`, {});
  }

  /**
   * Remove saved listing
   */
  public removeSavedListing(listingId: string): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/user/saved/${listingId}`);
  }

  /**
   * Get notifications
   */
  public getNotifications(page: number = 1, pageSize: number = 20): Observable<GetNotificationsResponse> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    
    return this.http.get<GetNotificationsResponse>(`${environment.apiUrl}/user/notifications`, { params });
  }

  /**
   * Mark notification as read
   */
  public markNotificationRead(notificationId: string): Observable<any> {
    return this.http.put(`${environment.apiUrl}/user/notifications/${notificationId}/read`, {});
  }

  /**
   * Mark all notifications as read
   */
  public markAllNotificationsRead(): Observable<any> {
    return this.http.put(`${environment.apiUrl}/user/notifications/read-all`, {});
  }

  /**
   * Get unread notification count
   */
  public getUnreadCount(): Observable<{ success: boolean; data: { count: number } }> {
    return this.http.get<{ success: boolean; data: { count: number } }>(`${environment.apiUrl}/user/notifications/unread-count`);
  }
}

