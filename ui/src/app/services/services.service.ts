import { Injectable, signal, computed } from '@angular/core';
import { ServiceCategoryDto } from '../models/dtos/service-category.dto';
import { MOCK_SERVICE_CATEGORIES } from '../data/mock-services.data';

interface ServicesState {
  categories: ServiceCategoryDto[];
  loading: boolean;
  error: string | null;
}

@Injectable({ providedIn: 'root' })
export class ServicesService {
  // Service categories state
  private readonly servicesState = signal<ServicesState>({
    categories: [],
    loading: false,
    error: null
  });
  public readonly services$ = computed(() => this.servicesState());

  /**
   * Fetch all service categories using mock data
   */
  public fetchServiceCategories(): void {
    this.servicesState.update(state => ({ ...state, loading: true, error: null }));

    // Use mock data directly
    const sortedCategories = [...MOCK_SERVICE_CATEGORIES].sort((a, b) => a.displayOrder - b.displayOrder);
    
    this.servicesState.set({
      categories: sortedCategories,
      loading: false,
      error: null
    });
  }
}

