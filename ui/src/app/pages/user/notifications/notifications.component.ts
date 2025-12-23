import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { UserApiService } from '../../../services/user.api';
import { catchError, of } from 'rxjs';

export interface Notification {
  id: string;
  title: string;
  message: string;
  type: 'booking' | 'payment' | 'reminder' | 'system';
  read: boolean;
  timestamp: string;
  actionUrl?: string;
}

@Component({
  selector: 'app-notifications',
  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './notifications.component.html',
  styleUrl: './notifications.component.scss'
})
export class NotificationsComponent implements OnInit {
  notifications = signal<Notification[]>([]);
  private userApiService = inject(UserApiService);

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    window.scrollTo(0, 0);
    this.loadNotifications();
  }

  private loadNotifications(): void {
    this.userApiService.getNotifications()
      .pipe(
        catchError(error => {
          console.error('Error loading notifications:', error);
          return of({ success: false, data: [] });
        })
      )
      .subscribe(response => {
        if (response.success) {
          const notifications = response.data.map(n => ({
            id: n.id,
            title: n.title,
            message: n.message,
            type: n.type,
            read: n.read,
            timestamp: n.timestamp,
            actionUrl: n.actionUrl
          }));
          this.notifications.set(notifications);
        }
      });
  }

  formatTimestamp(timestamp: string): string {
    const date = new Date(timestamp);
    const now = new Date();
    const diffMs = now.getTime() - date.getTime();
    const diffMins = Math.floor(diffMs / 60000);
    const diffHours = Math.floor(diffMs / 3600000);
    const diffDays = Math.floor(diffMs / 86400000);

    if (diffMins < 1) return 'Just now';
    if (diffMins < 60) return `${diffMins} minute${diffMins > 1 ? 's' : ''} ago`;
    if (diffHours < 24) return `${diffHours} hour${diffHours > 1 ? 's' : ''} ago`;
    if (diffDays < 7) return `${diffDays} day${diffDays > 1 ? 's' : ''} ago`;
    
    return date.toLocaleDateString('en-IN', {
      day: 'numeric',
      month: 'short',
      year: date.getFullYear() !== now.getFullYear() ? 'numeric' : undefined
    });
  }

  getNotificationIcon(type: string): string {
    switch (type) {
      case 'booking':
        return '📅';
      case 'payment':
        return '💳';
      case 'reminder':
        return '⏰';
      case 'system':
        return '🔔';
      default:
        return '📬';
    }
  }

  onMarkAsRead(notificationId: string): void {
    this.userApiService.markNotificationRead(notificationId)
      .pipe(
        catchError(error => {
          console.error('Error marking notification as read:', error);
          return of(null);
        })
      )
      .subscribe(response => {
        if (response) {
          this.notifications.update(notifs => 
            notifs.map(n => n.id === notificationId ? { ...n, read: true } : n)
          );
        }
      });
  }

  onMarkAllAsRead(): void {
    this.userApiService.markAllNotificationsRead()
      .pipe(
        catchError(error => {
          console.error('Error marking all notifications as read:', error);
          return of(null);
        })
      )
      .subscribe(response => {
        if (response) {
          this.notifications.update(notifs => 
            notifs.map(n => ({ ...n, read: true }))
          );
        }
      });
  }

  onDelete(notificationId: string): void {
    // Note: API doesn't have delete endpoint, so we just remove from local state
    this.notifications.update(notifs => notifs.filter(n => n.id !== notificationId));
  }

  onNotificationClick(notification: Notification): void {
    if (!notification.read) {
      this.onMarkAsRead(notification.id);
    }
    
    if (notification.actionUrl) {
      this.router.navigateByUrl(notification.actionUrl);
    }
  }

  getUnreadCount(): number {
    return this.notifications().filter(n => !n.read).length;
  }

  onLogout(): void {
    this.authService.logout();
  }
}

