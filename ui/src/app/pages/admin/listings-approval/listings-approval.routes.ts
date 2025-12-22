import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./listings-approval.component').then(m => m.AdminListingsApprovalComponent)
  }
];

