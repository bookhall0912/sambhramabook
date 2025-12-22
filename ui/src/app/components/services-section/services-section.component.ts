import { Component, OnInit, effect, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ServiceCardComponent } from '../service-card/service-card.component';
import { ServicesService } from '../../services/services.service';
import { ServiceCategoryDto } from '../../models/dtos/service-category.dto';

export interface Service {
  id: string;
  title: string;
  description: string;
  icon: string;
  iconUrl?: string | null;
  backgroundImageUrl?: string | null;
  themeColor?: string | null;
}

@Component({
  selector: 'app-services-section',
  imports: [CommonModule, ServiceCardComponent],
  templateUrl: './services-section.component.html',
  styleUrl: './services-section.component.scss'
})
export class ServicesSectionComponent implements OnInit {
  services = computed<Service[]>(() => {
    const categories = this.servicesService.services$().categories;
    // Show all services
    return categories.map(category => this.mapCategoryToService(category));
  });

  constructor(public servicesService: ServicesService) {
    // Fetch services when component initializes
    effect(() => {
      const state = this.servicesService.services$();
      // If not loading, no categories, and no error, fetch data
      if (!state.loading && state.categories.length === 0 && !state.error) {
        this.servicesService.fetchServiceCategories();
      }
    });
  }

  ngOnInit(): void {
    // Always fetch on init
    this.servicesService.fetchServiceCategories();
  }

  private mapCategoryToService(category: ServiceCategoryDto): Service {
    return {
      id: category.code,
      title: category.displayName,
      description: category.description || '',
      icon: category.code, // Use code as icon identifier
      iconUrl: category.iconUrl,
      backgroundImageUrl: category.backgroundImageUrl,
      themeColor: category.themeColor
    };
  }
}
