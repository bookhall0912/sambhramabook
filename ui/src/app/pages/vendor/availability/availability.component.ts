import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

interface CalendarDay {
  date: number;
  booked: boolean;
  selected: boolean;
}

@Component({
  selector: 'app-vendor-availability',
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './availability.component.html',
  styleUrl: './availability.component.scss'
})
export class VendorAvailabilityComponent implements OnInit {
  currentMonth = signal<string>('November 2024');
  calendarDays = signal<CalendarDay[]>([]);
  pendingBookingsCount = signal<number>(1);

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    window.scrollTo(0, 0);
    this.generateCalendar();
  }

  private generateCalendar(): void {
    const days: CalendarDay[] = [];
    const bookedDates = [5, 12, 15, 22, 25];
    for (let i = 1; i <= 30; i++) {
      days.push({
        date: i,
        booked: bookedDates.includes(i),
        selected: false
      });
    }
    this.calendarDays.set(days);
  }

  previousMonth(): void {
    // TODO: Implement month navigation
    console.log('Previous month');
  }

  nextMonth(): void {
    // TODO: Implement month navigation
    console.log('Next month');
  }

  onBulkUpdate(): void {
    // TODO: Open bulk update modal
    console.log('Bulk update');
  }

  onLogout(): void {
    this.authService.logout();
  }
}

