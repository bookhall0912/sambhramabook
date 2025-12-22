export interface BookingRequestDto {
  hallId: string;
  startDate: string; // ISO date string
  endDate: string; // ISO date string
  numberOfDays: number;
  guests: number;
  contactInfo: ContactInfoDto;
  specialRequests?: string;
}

export interface ContactInfoDto {
  name: string;
  email: string;
  phone: string;
}

