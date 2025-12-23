import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { environment } from 'environments/environment';
import { ServiceCategoriesResponseDto, ServiceCategoryDto } from '../models/dtos/service-category.dto';

export interface ServiceVendorDto {
  id: string;
  serviceType: number;
  title: string;
  description: string | null;
  city: string;
  location?: string;
  latitude?: number;
  longitude?: number;
  rating?: number;
  reviewCount?: number;
  price?: number;
  imageUrl?: string;
  images?: string[];
}

export interface GetServicesByTypeResponse {
  success: boolean;
  data: ServiceVendorDto[];
  pagination?: {
    page: number;
    pageSize: number;
    total: number;
    totalPages: number;
  };
}

export interface ServiceDetailDto {
  id: string;
  title: string;
  description: string;
  location: string;
  rating: number;
  reviewCount: number;
  price: number;
  imageUrl: string;
  images: string[];
  packages?: any[];
  portfolioImages?: string[];
  reviews?: any[];
}

@Injectable({ providedIn: 'root' })
export class ServicesApiService {
  private readonly http = inject(HttpClient);

  /**
   * Get all service categories
   * Handles both array response and object with categories property
   */
  public getServiceCategories(): Observable<ServiceCategoriesResponseDto> {
    return this.http.get<ServiceCategoryDto[] | ServiceCategoriesResponseDto>(`${environment.apiUrl}/services/categories`).pipe(
      map((response) => {
        // If response is an array, wrap it in an object
        if (Array.isArray(response)) {
          return { categories: response };
        }
        // If response has categories property, return as is
        if (response && 'categories' in response) {
          return response as ServiceCategoriesResponseDto;
        }
        // If response is empty or invalid, return empty array
        return { categories: [] };
      }),
      catchError((error) => {
        console.error('API error in ServicesApiService:', error);
        // Return empty response to trigger fallback in service
        return of({ categories: [] });
      })
    );
  }

  /**
   * Get services by type (photography, catering, etc.)
   */
  public getServicesByType(type: string, location?: string, page: number = 1, pageSize: number = 12): Observable<GetServicesByTypeResponse> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    
    if (location) {
      params = params.set('location', location);
    }
    
    return this.http.get<GetServicesByTypeResponse>(`${environment.apiUrl}/services/${type}`, { params });
  }

  /**
   * Get service vendor details
   */
  public getServiceDetails(type: string, id: string): Observable<{ success: boolean; data: ServiceDetailDto }> {
    return this.http.get<{ success: boolean; data: ServiceDetailDto }>(`${environment.apiUrl}/services/${type}/${id}`);
  }

  /**
   * Get service packages
   */
  public getServicePackages(type: string, id: string): Observable<any> {
    return this.http.get(`${environment.apiUrl}/services/${type}/${id}/packages`);
  }

  /**
   * Get service portfolio
   */
  public getServicePortfolio(type: string, id: string): Observable<any> {
    return this.http.get(`${environment.apiUrl}/services/${type}/${id}/portfolio`);
  }

  /**
   * Get service reviews
   */
  public getServiceReviews(type: string, id: string, page: number = 1, pageSize: number = 10): Observable<any> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    
    return this.http.get(`${environment.apiUrl}/services/${type}/${id}/reviews`, { params });
  }
}

