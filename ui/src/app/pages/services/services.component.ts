import { Component, OnInit, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HeaderComponent } from '../../components/header/header.component';
import { FooterComponent } from '../../components/footer/footer.component';
import { ServicesService } from '../../services/services.service';
import { ServiceCategoryDto } from '../../models/dtos/service-category.dto';

@Component({
  selector: 'app-services',
  imports: [CommonModule, HeaderComponent, FooterComponent],
  template: `
    <app-header></app-header>
    <div class="services-page">
      <div class="services-header">
        <h1>Services</h1>
        <p>Explore our range of event services.</p>
      </div>
      
      @if (servicesService.services$().loading) {
        <div class="loading">Loading services...</div>
      } @else if (servicesService.services$().error) {
        <div class="error">{{ servicesService.services$().error }}</div>
      } @else {
        <div class="services-grid">
          @for (category of servicesService.services$().categories; track category.code) {
            <div class="service-card" [style.background-color]="category.themeColor || '#f0f0f0'" (click)="onServiceClick(category.code)">
              @if (category.backgroundImageUrl) {
                <div class="service-background" [style.background-image]="'url(' + category.backgroundImageUrl + ')'"></div>
              }
              <div class="service-content">
                @if (category.iconUrl) {
                  <img [src]="category.iconUrl" [alt]="category.displayName" class="service-icon" />
                }
                <h3 class="service-title">{{ category.displayName }}</h3>
                @if (category.description) {
                  <p class="service-description">{{ category.description }}</p>
                }
              </div>
            </div>
          }
        </div>
      }
    </div>
    <app-footer></app-footer>
  `,
  styles: [`
    .services-page {
      padding: 2rem;
      max-width: 1200px;
      margin: 0 auto;
    }

    .services-header {
      text-align: center;
      margin-bottom: 3rem;
    }

    .services-header h1 {
      font-size: 2.5rem;
      margin-bottom: 1rem;
    }

    .services-header p {
      font-size: 1.1rem;
      color: #666;
    }

    .loading, .error {
      text-align: center;
      padding: 2rem;
      font-size: 1.1rem;
    }

    .error {
      color: #e74c3c;
    }

    .services-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
      gap: 2rem;
    }

    .service-card {
      position: relative;
      border-radius: 12px;
      overflow: hidden;
      min-height: 250px;
      cursor: pointer;
      transition: transform 0.3s ease, box-shadow 0.3s ease;
      box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    }

    .service-card:hover {
      transform: translateY(-5px);
      box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
    }

    .service-background {
      position: absolute;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0;
      background-size: cover;
      background-position: center;
      opacity: 0.3;
    }

    .service-content {
      position: relative;
      padding: 2rem;
      height: 100%;
      display: flex;
      flex-direction: column;
      justify-content: center;
      align-items: center;
      text-align: center;
      color: white;
      z-index: 1;
    }

    .service-icon {
      width: 80px;
      height: 80px;
      border-radius: 50%;
      margin-bottom: 1rem;
      object-fit: cover;
    }

    .service-title {
      font-size: 1.5rem;
      margin-bottom: 0.5rem;
      font-weight: 600;
    }

    .service-description {
      font-size: 1rem;
      opacity: 0.9;
      line-height: 1.5;
    }
  `]
})
export class ServicesComponent implements OnInit {
  constructor(
    public servicesService: ServicesService,
    private router: Router
  ) {
    // Fetch services when component initializes
    effect(() => {
      const state = this.servicesService.services$();
      if (!state.loading && state.categories.length === 0 && !state.error) {
        this.servicesService.fetchServiceCategories();
      }
    });
  }

  ngOnInit(): void {
    this.servicesService.fetchServiceCategories();
  }

  onServiceClick(serviceCode: string): void {
    // Convert service code to lowercase for URL (e.g., "PHOTOGRAPHY" -> "photography")
    const serviceType = serviceCode.toLowerCase();
    this.router.navigate(['/services', serviceType]);
  }
}
