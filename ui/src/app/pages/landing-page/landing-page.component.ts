import { Component } from '@angular/core';
import { HeaderComponent } from '../../components/header/header.component';
import { HeroSectionComponent } from '../../components/hero-section/hero-section.component';
import { FeatureBarComponent } from '../../components/feature-bar/feature-bar.component';
import { PopularVenuesComponent } from '../../components/popular-venues/popular-venues.component';
import { ServicesSectionComponent } from '../../components/services-section/services-section.component';
import { VendorCtaComponent } from '../../components/vendor-cta/vendor-cta.component';
import { FooterComponent } from '../../components/footer/footer.component';

@Component({
  selector: 'app-landing-page',
  imports: [
    HeaderComponent,
    HeroSectionComponent,
    FeatureBarComponent,
    PopularVenuesComponent,
    ServicesSectionComponent,
    VendorCtaComponent,
    FooterComponent
  ],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.scss'
})
export class LandingPageComponent {}
