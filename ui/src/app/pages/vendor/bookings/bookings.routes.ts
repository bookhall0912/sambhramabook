import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./bookings.component').then(m => m.VendorBookingsComponent)
  }
];

