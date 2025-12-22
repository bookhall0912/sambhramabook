import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

export interface Review {
  id: string;
  userName: string;
  userAvatar: string;
  venueName: string;
  rating: number;
  comment: string;
  date: string;
  status: 'APPROVED' | 'PENDING' | 'FLAGGED';
}

@Component({
  selector: 'app-admin-reports',
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './reports.component.html',
  styleUrl: './reports.component.scss'
})
export class AdminReportsComponent implements OnInit {
  reviews = signal<Review[]>([]);
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
    this.loadReviews();
  }

  private loadReviews(): void {
    this.reviews.set([
      {
        id: '1',
        userName: 'Aditi Sharma',
        userAvatar: 'https://i.pravatar.cc/32?img=47',
        venueName: 'Golden Palms Convention Hall',
        rating: 5,
        comment: 'Excellent venue with great amenities!',
        date: '2024-12-15',
        status: 'APPROVED'
      },
      {
        id: '2',
        userName: 'Rahul Kumar',
        userAvatar: 'https://i.pravatar.cc/32?img=12',
        venueName: 'Grand Royal Banquet',
        rating: 4,
        comment: 'Good service but parking was limited.',
        date: '2024-12-18',
        status: 'APPROVED'
      },
      {
        id: '3',
        userName: 'Priya Patel',
        userAvatar: 'https://i.pravatar.cc/32?img=22',
        venueName: 'Royal Garden Venue',
        rating: 1,
        comment: 'Very poor experience, not recommended.',
        date: '2024-12-20',
        status: 'FLAGGED'
      }
    ]);
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-IN', {
      day: 'numeric',
      month: 'short',
      year: 'numeric'
    });
  }

  onApprove(reviewId: string): void {
    this.reviews.update(reviews =>
      reviews.map(r => r.id === reviewId ? { ...r, status: 'APPROVED' as const } : r)
    );
  }

  onFlag(reviewId: string): void {
    this.reviews.update(reviews =>
      reviews.map(r => r.id === reviewId ? { ...r, status: 'FLAGGED' as const } : r)
    );
  }

  onDelete(reviewId: string): void {
    this.reviews.update(reviews => reviews.filter(r => r.id !== reviewId));
  }

  onLogout(): void {
    this.authService.logout();
  }
}

