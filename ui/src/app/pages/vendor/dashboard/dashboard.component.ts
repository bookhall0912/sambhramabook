import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-vendor-dashboard',
  imports: [CommonModule],
  template: `
    <div class="dashboard-page">
      <h1>Vendor Dashboard</h1>
      <p>Welcome to your vendor dashboard. Design will be added later.</p>
      <button (click)="goBack()">Go Back</button>
    </div>
  `,
  styles: [`
    .dashboard-page {
      padding: var(--spacing-2xl);
      text-align: center;
    }
  `]
})
export class VendorDashboardComponent implements OnInit {
  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    // Check if user is vendor
    if (!this.authService.hasRole('Vendor')) {
      this.router.navigate(['/']);
    }
    window.scrollTo(0, 0);
  }

  goBack(): void {
    this.router.navigate(['/']);
  }
}

