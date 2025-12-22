import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./bookings.component').then(m => m.BookingsComponent)
  },
  {
    path: 'saved',
    loadChildren: () => import('../saved/saved.routes').then(m => m.routes)
  },
  {
    path: 'profile',
    loadChildren: () => import('../profile/profile.routes').then(m => m.routes)
  },
  {
    path: 'notifications',
    loadChildren: () => import('../notifications/notifications.routes').then(m => m.routes)
  }
];

