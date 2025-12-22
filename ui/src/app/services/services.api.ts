import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { environment } from 'environments/environment';
import { ServiceCategoriesResponseDto, ServiceCategoryDto } from '../models/dtos/service-category.dto';

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
}

