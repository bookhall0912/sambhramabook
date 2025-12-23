import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';

export interface PendingListingDto {
  id: string;
  name: string;
  location: string;
  vendorName: string;
  vendorAvatar: string;
  type: 'Hall' | 'Service';
  submitted: string;
  status: 'PENDING' | 'NEEDS_CHANGES';
}

export interface GetPendingListingsResponse {
  success: boolean;
  data: PendingListingDto[];
  pagination?: {
    page: number;
    pageSize: number;
    total: number;
    totalPages: number;
  };
}

export interface AdminSettingDto {
  key: string;
  value: string;
  description?: string;
}

export interface GetSettingsResponse {
  success: boolean;
  data: AdminSettingDto[];
}

@Injectable({ providedIn: 'root' })
export class AdminApiService {
  private readonly http = inject(HttpClient);

  /**
   * Get pending listings for approval
   */
  public getPendingListings(page: number = 1, pageSize: number = 10): Observable<GetPendingListingsResponse> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    
    return this.http.get<GetPendingListingsResponse>(`${environment.apiUrl}/admin/listings/pending`, { params });
  }

  /**
   * Approve listing
   */
  public approveListing(listingId: string): Observable<any> {
    return this.http.put(`${environment.apiUrl}/admin/listings/${listingId}/approve`, {});
  }

  /**
   * Reject listing
   */
  public rejectListing(listingId: string, reason?: string): Observable<any> {
    return this.http.put(`${environment.apiUrl}/admin/listings/${listingId}/reject`, { reason });
  }

  /**
   * Request listing changes
   */
  public requestListingChanges(listingId: string, changes: string[]): Observable<any> {
    return this.http.put(`${environment.apiUrl}/admin/listings/${listingId}/request-changes`, { changes });
  }

  /**
   * Get admin settings
   */
  public getSettings(): Observable<GetSettingsResponse> {
    return this.http.get<GetSettingsResponse>(`${environment.apiUrl}/admin/settings`);
  }

  /**
   * Update admin setting
   */
  public updateSetting(key: string, value: string): Observable<any> {
    return this.http.put(`${environment.apiUrl}/admin/settings/${key}`, { value });
  }
}

