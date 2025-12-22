import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-admin-analytics',
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './analytics.component.html',
  styleUrl: './analytics.component.scss'
})
export class AdminAnalyticsComponent implements OnInit {
  pendingCount = signal<number>(5);
  payoutsCount = signal<number>(2);

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    window.scrollTo(0, 0);
    if (!this.authService.hasRole('Admin')) {
      this.router.navigate(['/']);
    }
  }

  onLogout(): void {
    this.authService.logout();
  }
}

