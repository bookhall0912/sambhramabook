import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

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
  notifications = signal<Notification[]>([
    {
      id: '1',
      title: 'Booking Confirmed',
      message: 'Your booking for Sambhrama Grand Hall has been confirmed.',
      type: 'booking',
      read: false,
      timestamp: '2024-11-10T10:30:00',
      actionUrl: '/booking/confirmation?bookingId=SB-8824-X901'
    },
    {
      id: '2',
      title: 'Payment Received',
      message: 'Payment of ₹3,06,425 has been successfully processed.',
      type: 'payment',
      read: false,
      timestamp: '2024-11-10T09:15:00',
      actionUrl: '/bookings'
    },
    {
      id: '3',
      title: 'Event Reminder',
      message: 'Your event at Sambhrama Grand Hall is in 2 days.',
      type: 'reminder',
      read: true,
      timestamp: '2024-11-08T14:20:00',
      actionUrl: '/bookings'
    },
    {
      id: '4',
      title: 'System Update',
      message: 'New features have been added to your dashboard.',
      type: 'system',
      read: true,
      timestamp: '2024-11-05T08:00:00'
    }
  ]);

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    window.scrollTo(0, 0);
    // TODO: Load notifications from API
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
    // TODO: Call API to mark as read
    this.notifications.update(notifs => 
      notifs.map(n => n.id === notificationId ? { ...n, read: true } : n)
    );
  }

  onMarkAllAsRead(): void {
    // TODO: Call API to mark all as read
    this.notifications.update(notifs => 
      notifs.map(n => ({ ...n, read: true }))
    );
  }

  onDelete(notificationId: string): void {
    // TODO: Call API to delete notification
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

