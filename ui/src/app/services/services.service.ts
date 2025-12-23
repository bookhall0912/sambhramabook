import { Injectable, signal, computed, inject } from '@angular/core';
import { ServiceCategoryDto } from '../models/dtos/service-category.dto';
import { ServicesApiService } from './services.api';
import { catchError, of } from 'rxjs';

interface ServicesState {
  categories: ServiceCategoryDto[];
  loading: boolean;
  error: string | null;
}

@Injectable({ providedIn: 'root' })
export class ServicesService {
  private readonly servicesApiService = inject(ServicesApiService);

  // Service categories state
  private readonly servicesState = signal<ServicesState>({
    categories: [],
    loading: false,
    error: null
  });
  public readonly services$ = computed(() => this.servicesState());

  /**
   * Fetch all service categories from API
   */
  public fetchServiceCategories(): void {
    this.servicesState.update(state => ({ ...state, loading: true, error: null }));

    this.servicesApiService.getServiceCategories()
      .pipe(
        catchError(error => {
          console.error('Error fetching service categories:', error);
          this.servicesState.update(state => ({
            ...state,
            loading: false,
            error: 'Failed to load service categories. Please try again.'
          }));
          return of({ categories: [] });
        })
      )
      .subscribe(response => {
        const sortedCategories = [...response.categories].sort((a, b) => a.displayOrder - b.displayOrder);
        this.servicesState.set({
          categories: sortedCategories,
          loading: false,
          error: null
        });
      });
  }
}

