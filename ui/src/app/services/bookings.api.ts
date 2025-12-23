import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';

export interface BookingDetailDto {
  id: string;
  bookingId: string;
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
  contactInfo?: {
    name: string;
    email: string;
    phone: string;
  };
  specialRequests?: string;
  paymentMethod?: string;
  paymentTransactionId?: string;
  paymentDate?: string;
}

@Injectable({ providedIn: 'root' })
export class BookingsApiService {
  private readonly http = inject(HttpClient);

  /**
   * Get booking details by ID
   */
  public getBookingById(id: string): Observable<BookingDetailDto> {
    return this.http.get<BookingDetailDto>(`${environment.apiUrl}/bookings/${id}`);
  }

  /**
   * Get booking details by reference number
   */
  public getBookingByReference(reference: string): Observable<BookingDetailDto> {
    return this.http.get<BookingDetailDto>(`${environment.apiUrl}/bookings/reference/${reference}`);
  }

  /**
   * Cancel booking
   */
  public cancelBooking(id: string, reason?: string): Observable<any> {
    return this.http.put(`${environment.apiUrl}/bookings/${id}/cancel`, { reason });
  }

  /**
   * Reschedule booking
   */
  public rescheduleBooking(id: string, newStartDate: string, newEndDate: string, reason?: string): Observable<any> {
    return this.http.put(`${environment.apiUrl}/bookings/${id}/reschedule`, {
      newStartDate,
      newEndDate,
      reason
    });
  }
}

