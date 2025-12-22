import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./overview/overview.component').then(m => m.AdminOverviewComponent)
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./overview/overview.component').then(m => m.AdminOverviewComponent)
  },
  {
    path: 'analytics',
    loadChildren: () => import('../analytics/analytics.routes').then(m => m.routes)
  },
  {
    path: 'users',
    loadChildren: () => import('../users/users.routes').then(m => m.routes)
  },
  {
    path: 'vendors',
    loadChildren: () => import('../vendors/vendors.routes').then(m => m.routes)
  },
  {
    path: 'listings-approval',
    loadChildren: () => import('../listings-approval/listings-approval.routes').then(m => m.routes)
  },
  {
    path: 'bookings',
    loadChildren: () => import('../bookings/bookings.routes').then(m => m.routes)
  },
  {
    path: 'payouts',
    loadChildren: () => import('../payouts/payouts.routes').then(m => m.routes)
  },
  {
    path: 'reports',
    loadChildren: () => import('../reports/reports.routes').then(m => m.routes)
  },
  {
    path: 'settings',
    loadChildren: () => import('../settings/settings.routes').then(m => m.routes)
  }
];

