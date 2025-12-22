import { HallDto } from './hall.dto';
import { ReviewDto } from './review.dto';

export interface HallDetailResponseDto extends HallDto {
  fullDescription: string;
  fullAmenities: string[];
  reviews: ReviewDto[];
  policies?: string[];
  cancellationPolicy?: string;
  checkInTime?: string;
  checkOutTime?: string;
}

