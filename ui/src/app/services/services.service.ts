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
    // Prevent multiple simultaneous requests
    const currentState = this.servicesState();
    if (currentState.loading) {
      return;
    }

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
          // Return EMPTY observable to stop the chain, don't emit empty categories
          return of({ categories: [] });
        })
      )
      .subscribe({
        next: (response) => {
          const currentState = this.servicesState();
          const sortedCategories = [...response.categories].sort((a, b) => a.displayOrder - b.displayOrder);
          
          // Only clear error if we actually got categories
          // If categories is empty, preserve any existing error
          const errorToSet = sortedCategories.length > 0 ? null : currentState.error;
          
          this.servicesState.set({
            categories: sortedCategories,
            loading: false,
            error: errorToSet
          });
        },
        error: (error) => {
          // This should not be reached due to catchError, but handle it just in case
          console.error('Unexpected error in fetchServiceCategories:', error);
          this.servicesState.update(state => ({
            ...state,
            loading: false,
            error: 'Failed to load service categories. Please try again.'
          }));
        }
      });
  }
}

