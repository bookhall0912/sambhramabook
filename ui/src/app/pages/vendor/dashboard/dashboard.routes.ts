import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./overview/overview.component').then(m => m.VendorOverviewComponent)
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./overview/overview.component').then(m => m.VendorOverviewComponent)
  },
  {
    path: 'listings',
    loadChildren: () => import('../listings/listings.routes').then(m => m.routes)
  },
  {
    path: 'bookings',
    loadChildren: () => import('../bookings/bookings.routes').then(m => m.routes)
  },
  {
    path: 'availability',
    loadChildren: () => import('../availability/availability.routes').then(m => m.routes)
  },
  {
    path: 'earnings',
    loadChildren: () => import('../earnings/earnings.routes').then(m => m.routes)
  },
  {
    path: 'settings',
    loadChildren: () => import('../settings/settings.routes').then(m => m.routes)
  }
];

