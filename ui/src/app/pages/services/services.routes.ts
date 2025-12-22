import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./services.component').then(m => m.ServicesComponent)
  },
  {
    path: ':type',
    loadComponent: () => import('./service-listing/service-listing.component').then(m => m.ServiceListingComponent)
  },
  {
    path: ':type/:id',
    loadComponent: () => import('./service-detail/service-detail.component').then(m => m.ServiceDetailComponent)
  }
];
