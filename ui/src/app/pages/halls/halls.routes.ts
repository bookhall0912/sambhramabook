import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./halls.component').then(m => m.HallsComponent)
  },
  {
    path: ':id',
    loadComponent: () => import('./hall-detail/hall-detail.component').then(m => m.HallDetailComponent)
  }
];
