/**
 * Service Get Response DTO from /services endpoint
 * The API returns an array of these objects directly
 * 
 * NOTE: Many fields are missing and will be added later.
 * See MISSING_API_FIELDS.md for complete list of required fields.
 */
export interface ServiceGetResponseDto {
  id: string;
  serviceType: number; // ServiceType enum: 1 = Hall
  title: string;
  description: string | null;
  city: string;
  // Missing fields - will be added to API later:
  latitude?: number;
  longitude?: number;
  rating?: number;
  reviewCount?: number;
  capacity?: number;
  minCapacity?: number;
  maxCapacity?: number;
  rooms?: number;
  price?: number;
  imageUrl?: string;
  images?: string[];
  amenities?: string[];
  parking?: string;
  location?: string; // Full location string (more than just city)
}

