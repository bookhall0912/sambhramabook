import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

export interface User {
  id: string;
  name: string;
  email: string;
  mobile: string;
  avatar: string;
  status: 'ACTIVE' | 'INACTIVE' | 'SUSPENDED';
  joinDate: string;
  bookings: number;
}

@Component({
  selector: 'app-admin-users',
  imports: [CommonModule, FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './users.component.html',
  styleUrl: './users.component.scss'
})
export class AdminUsersComponent implements OnInit {
  users = signal<User[]>([]);
  pendingCount = signal<number>(5);
  payoutsCount = signal<number>(2);
  searchTerm = signal<string>('');

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    window.scrollTo(0, 0);
    if (!this.authService.hasRole('Admin')) {
      this.router.navigate(['/']);
    }
    this.loadUsers();
  }

  private loadUsers(): void {
    this.users.set([
      {
        id: '1',
        name: 'Aditi Sharma',
        email: 'aditi@example.com',
        mobile: '+91 98765 43210',
        avatar: 'https://i.pravatar.cc/32?img=47',
        status: 'ACTIVE',
        joinDate: '2024-01-15',
        bookings: 12
      },
      {
        id: '2',
        name: 'Rahul Kumar',
        email: 'rahul@example.com',
        mobile: '+91 98765 43211',
        avatar: 'https://i.pravatar.cc/32?img=12',
        status: 'ACTIVE',
        joinDate: '2024-02-20',
        bookings: 8
      },
      {
        id: '3',
        name: 'Priya Patel',
        email: 'priya@example.com',
        mobile: '+91 98765 43212',
        avatar: 'https://i.pravatar.cc/32?img=22',
        status: 'INACTIVE',
        joinDate: '2024-03-10',
        bookings: 3
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

  onViewDetails(userId: string): void {
    console.log('View user details:', userId);
  }

  onSuspend(userId: string): void {
    this.users.update(users =>
      users.map(u => u.id === userId ? { ...u, status: 'SUSPENDED' as const } : u)
    );
  }

  onLogout(): void {
    this.authService.logout();
  }
}

