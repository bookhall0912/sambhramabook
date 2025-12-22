import { Component, input, output, signal, WritableSignal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

export interface BookingData {
  id: string;
  bookingId: string;
  venueName: string;
  venueImage?: string;
  date: string;
  location?: string;
  totalAmount: number;
  cancellationFee?: number;
  estimatedRefund?: number;
}

export type ActionType = 'cancel' | 'reschedule';

@Component({
  selector: 'app-manage-booking-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './manage-booking-modal.component.html',
  styleUrl: './manage-booking-modal.component.scss'
})
export class ManageBookingModalComponent {
  booking = input<BookingData | null>(null);
  visible = input.required<WritableSignal<boolean>>();
  close = output<void>();
  confirmCancel = output<{ bookingId: string; reason: string }>();
  confirmReschedule = output<{ bookingId: string }>();
  keepBooking = output<void>();

  selectedAction = signal<ActionType>('cancel');
  cancellationReason = signal<string>('');

  cancellationReasons = [
    'Event postponed',
    'Change of plans',
    'Found alternative venue',
    'Other'
  ];

  onClose(): void {
    this.visible().set(false);
    this.close.emit();
  }

  onSelectAction(action: ActionType): void {
    this.selectedAction.set(action);
  }

  onKeepBooking(): void {
    this.keepBooking.emit();
    this.onClose();
  }

  onConfirm(): void {
    const bookingData = this.booking();
    if (!bookingData) return;

    if (this.selectedAction() === 'cancel') {
      if (!this.cancellationReason()) {
        alert('Please select a reason for cancellation');
        return;
      }
      this.confirmCancel.emit({
        bookingId: bookingData.id,
        reason: this.cancellationReason()
      });
    } else {
      this.confirmReschedule.emit({
        bookingId: bookingData.id
      });
    }
    this.onClose();
  }

  calculateRefund(): number {
    const bookingData = this.booking();
    if (!bookingData) return 0;
    const fee = bookingData.cancellationFee || (bookingData.totalAmount * 0.5);
    return bookingData.totalAmount - fee;
  }

  getCancellationFee(): number {
    const bookingData = this.booking();
    if (!bookingData) return 0;
    return bookingData.cancellationFee || (bookingData.totalAmount * 0.5);
  }

  formatPrice(amount: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR',
      maximumFractionDigits: 0
    }).format(amount);
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-IN', {
      day: 'numeric',
      month: 'short',
      year: 'numeric'
    });
  }
}

