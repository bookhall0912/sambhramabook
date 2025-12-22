import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./listings.component').then(m => m.VendorListingsComponent)
  },
  {
    path: 'new',
    loadComponent: () => import('./add-edit/add-edit.component').then(m => m.VendorAddEditListingComponent)
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./add-edit/add-edit.component').then(m => m.VendorAddEditListingComponent)
  }
];

