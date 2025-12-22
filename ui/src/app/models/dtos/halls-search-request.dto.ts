export interface HallsSearchRequestDto {
  location?: string;
  minPrice?: number;
  maxPrice?: number;
  minCapacity?: number;
  maxCapacity?: number;
  amenities?: string[];
  page?: number;
  pageSize?: number;
  date?: string; // ISO date string
  days?: number;
  guests?: number;
}

