import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./availability.component').then(m => m.VendorAvailabilityComponent)
  }
];

