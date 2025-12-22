import { Component, input, output, computed } from '@angular/core';
import { CommonModule } from '@angular/common';

export type DayStatus = 'available' | 'booked' | 'selected';

export interface CalendarDay {
  day: number;
  status: DayStatus;
}

interface CalendarCell {
  day: number | null;
  status: DayStatus | null;
}

@Component({
  selector: 'app-availability-calendar',
  imports: [CommonModule],
  templateUrl: './availability-calendar.component.html',
  styleUrl: './availability-calendar.component.scss'
})
export class AvailabilityCalendarComponent {
  month = input<string>('October 2025');
  days = input<CalendarDay[]>([]);
  selectedDay = input<number | null>(null);
  daySelected = output<number>();

  dayHeaders = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];

  calendarGrid = computed(() => {
    const monthStr = this.month();
    const [monthName, yearStr] = monthStr.split(' ');
    const year = parseInt(yearStr, 10);
    const monthIndex = new Date(`${monthName} 1, ${year}`).getMonth();
    
    const firstDay = new Date(year, monthIndex, 1).getDay();
    const daysInMonth = new Date(year, monthIndex + 1, 0).getDate();
    
    const daysMap = new Map(this.days().map(d => [d.day, d.status]));
    const selected = this.selectedDay();
    
    const grid: CalendarCell[] = [];
    
    // Add empty cells for days before the first day of the month
    for (let i = 0; i < firstDay; i++) {
      grid.push({ day: null, status: null });
    }
    
    // Add all days of the month
    for (let day = 1; day <= daysInMonth; day++) {
      let status: DayStatus = 'available';
      if (selected === day) {
        status = 'selected';
      } else if (daysMap.has(day)) {
        status = daysMap.get(day)!;
      }
      grid.push({ day, status });
    }
    
    return grid;
  });

  onDayClick(day: number, status: DayStatus | null): void {
    if (status && status !== 'booked') {
      this.daySelected.emit(day);
    }
  }
}

