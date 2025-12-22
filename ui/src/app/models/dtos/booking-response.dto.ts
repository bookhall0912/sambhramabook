export interface BookingResponseDto {
  bookingId: string;
  hallId: string;
  hallName: string;
  startDate: string;
  endDate: string;
  numberOfDays: number;
  guests: number;
  totalAmount: number;
  status: 'pending' | 'confirmed' | 'cancelled';
  confirmationNumber: string;
  message?: string;
}

