import { Component, input, output, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DatePickerComponent } from '../date-picker/date-picker.component';

export interface BookingData {
  date: string;
  days: number;
  guests: number;
}

@Component({
  selector: 'app-booking-card',
  imports: [CommonModule, FormsModule, DatePickerComponent],
  templateUrl: './booking-card.component.html',
  styleUrl: './booking-card.component.scss'
})
export class BookingCardComponent implements OnInit {
  price = input.required<number>();
  bookingSubmitted = output<BookingData>();

  selectedDate: string;
  numberOfDays = 1;
  guests = 800;
  venueRental = 0;
  taxes = 0;
  total = 0;
  readonly minDate = new Date();

  constructor() {
    // Set today's date as default
    this.selectedDate = this.formatDate(new Date());
  }

  ngOnInit(): void {
    this.calculateTotal();
  }

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  formatPrice(price: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR',
      maximumFractionDigits: 0
    }).format(price);
  }

  calculateTotal(): void {
    this.venueRental = this.price() * this.numberOfDays;
    this.taxes = Math.round(this.venueRental * 0.18);
    this.total = this.venueRental + this.taxes;
  }

  onDaysChange(): void {
    this.calculateTotal();
  }

  onSubmit(): void {
    this.bookingSubmitted.emit({
      date: this.selectedDate,
      days: this.numberOfDays,
      guests: this.guests
    });
  }
}

