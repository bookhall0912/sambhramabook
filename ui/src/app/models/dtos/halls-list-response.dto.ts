import { HallDto } from './hall.dto';

export interface HallsListResponseDto {
  halls: HallDto[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

