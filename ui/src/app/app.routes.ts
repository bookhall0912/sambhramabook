import { Routes } from '@angular/router';
import { LandingPageComponent } from './pages/landing-page/landing-page.component';

export const routes: Routes = [
  {
    path: '',
    component: LandingPageComponent
  },
  {
    path: 'halls',
    loadChildren: () => import('./pages/halls/halls.routes').then(m => m.routes)
  },
  {
    path: 'services',
    loadChildren: () => import('./pages/services/services.routes').then(m => m.routes)
  },
  {
    path: 'booking/confirmation',
    loadChildren: () => import('./pages/booking/confirmation/confirmation.routes').then(m => m.routes)
  },
  {
    path: 'booking',
    loadChildren: () => import('./pages/booking/booking.routes').then(m => m.routes)
  },
  {
    path: 'bookings',
    loadChildren: () => import('./pages/user/bookings/bookings.routes').then(m => m.routes)
  },
  {
    path: 'login',
    loadChildren: () => import('./pages/auth/login/login.routes').then(m => m.routes)
  },
  {
    path: 'signup',
    loadChildren: () => import('./pages/auth/register/register.routes').then(m => m.routes)
  },
  {
    path: 'vendor/onboarding',
    loadComponent: () => import('./pages/vendor/onboarding/onboarding.component').then(m => m.VendorOnboardingComponent)
  },
  {
    path: 'vendor/dashboard',
    loadChildren: () => import('./pages/vendor/dashboard/dashboard.routes').then(m => m.routes)
  },
  {
    path: 'admin',
    loadChildren: () => import('./pages/admin/dashboard/dashboard.routes').then(m => m.routes)
  },
  {
    path: 'vendors',
    loadComponent: () => import('./pages/vendors/vendors.component').then(m => m.VendorsComponent)
  }
];

