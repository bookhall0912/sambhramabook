export interface HallDto {
  id: string;
  name: string;
  location: string;
  distance?: string;
  rating: number;
  reviewCount: number;
  capacity: number;
  minCapacity?: number;
  maxCapacity?: number;
  rooms: number;
  price: number;
  imageUrl: string;
  images?: string[];
  amenities: string[];
  description?: string;
  parking?: string;
  latitude?: number;
  longitude?: number;
}

