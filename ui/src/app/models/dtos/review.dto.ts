export interface ReviewDto {
  id: string;
  author: string;
  rating: number;
  comment: string;
  date: string;
  verified?: boolean;
}

