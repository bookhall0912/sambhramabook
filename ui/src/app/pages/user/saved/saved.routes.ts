import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./saved.component').then(m => m.SavedComponent)
  }
];

