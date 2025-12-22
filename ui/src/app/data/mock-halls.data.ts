import { HallDto } from '../models/dtos/hall.dto';
import { HallDetailResponseDto } from '../models/dtos/hall-detail-response.dto';
import { ReviewDto } from '../models/dtos/review.dto';

/**
 * Mock data for development purposes.
 * TODO: Remove this file once backend APIs are fully implemented.
 */

export const MOCK_HALLS: HallDto[] = [
  {
    id: '1',
    name: 'Royal Palace Convention Center',
    location: 'Jayanagar, Bangalore',
    distance: '2.5 km',
    rating: 4.9,
    reviewCount: 128,
    capacity: 1200,
    minCapacity: 800,
    maxCapacity: 1200,
    rooms: 15,
    price: 150000,
    imageUrl: 'https://images.unsplash.com/photo-1528605248644-14dd04022da1?w=800&h=600&fit=crop',
    images: [
      'https://images.unsplash.com/photo-1528605248644-14dd04022da1?w=800&h=600&fit=crop',
      'https://images.unsplash.com/photo-1522335789203-aabd1fc54bc9?w=800&h=600&fit=crop',
      'https://images.unsplash.com/photo-1519741497674-611481863552?w=800&h=600&fit=crop'
    ],
    amenities: ['AC'],
    description: 'Royal Palace Convention Center stands as one of Bangalore\'s most prestigious wedding venues.',
    parking: '100+ Cars Parking',
    latitude: 12.9716,
    longitude: 77.5946
  },
  {
    id: '2',
    name: 'Sree Krishna Convention Hall',
    location: 'Malleswaram, Bangalore',
    distance: '5.2 km',
    rating: 4.6,
    reviewCount: 95,
    capacity: 500,
    minCapacity: 300,
    maxCapacity: 500,
    rooms: 6,
    price: 75000,
    imageUrl: 'https://images.unsplash.com/photo-1519741497674-611481863552?w=800&h=600&fit=crop',
    amenities: [],
    description: 'A traditional venue perfect for intimate celebrations.',
    parking: '50+ Cars Parking',
    latitude: 12.9981,
    longitude: 77.5708
  },
  {
    id: '3',
    name: 'Golden Pearl Grand',
    location: 'Mysore Road, Bangalore',
    distance: '12 km',
    rating: 4.7,
    reviewCount: 112,
    capacity: 800,
    minCapacity: 500,
    maxCapacity: 800,
    rooms: 10,
    price: 95000,
    imageUrl: 'https://images.unsplash.com/photo-1522335789203-aabd1fc54bc9?w=800&h=600&fit=crop',
    amenities: ['AC'],
    description: 'Elegant venue with modern amenities and spacious halls.',
    parking: '80+ Cars Parking',
    latitude: 12.9352,
    longitude: 77.5345
  },
  {
    id: '4',
    name: 'Elegant Events Venue',
    location: 'Whitefield, Bangalore',
    distance: '18 km',
    rating: 4.8,
    reviewCount: 145,
    capacity: 1000,
    minCapacity: 600,
    maxCapacity: 1000,
    rooms: 12,
    price: 135000,
    imageUrl: 'https://images.unsplash.com/photo-1464366400600-7168b8af9bc3?w=800&h=600&fit=crop',
    amenities: ['AC', 'Valet Parking'],
    description: 'Premium venue with world-class facilities and excellent service.',
    parking: '120+ Cars Parking',
    latitude: 12.9698,
    longitude: 77.7499
  },
  {
    id: '5',
    name: 'Grand Celebration Hall',
    location: 'Indiranagar, Bangalore',
    distance: '3.8 km',
    rating: 4.7,
    reviewCount: 98,
    capacity: 800,
    minCapacity: 500,
    maxCapacity: 800,
    rooms: 10,
    price: 120000,
    imageUrl: 'https://images.unsplash.com/photo-1511578314322-379afb476865?w=800&h=600&fit=crop',
    amenities: ['AC', 'Guest Rooms'],
    description: 'Spacious hall with modern amenities and professional catering services.',
    parking: '70+ Cars Parking',
    latitude: 12.9784,
    longitude: 77.6408
  },
  {
    id: '6',
    name: 'Platinum Convention Center',
    location: 'Koramangala, Bangalore',
    distance: '4.5 km',
    rating: 4.9,
    reviewCount: 167,
    capacity: 1500,
    minCapacity: 1000,
    maxCapacity: 1500,
    rooms: 20,
    price: 180000,
    imageUrl: 'https://images.unsplash.com/photo-1519167758481-83f550bb49b3?w=800&h=600&fit=crop',
    amenities: ['AC', 'Valet Parking', 'Guest Rooms'],
    description: 'Luxury venue with state-of-the-art facilities and premium services.',
    parking: '200+ Cars Parking',
    latitude: 12.9352,
    longitude: 77.6245
  }
];

export const MOCK_HALL_DETAILS: Record<string, HallDetailResponseDto> = {
  '1': {
    id: '1',
    name: 'Royal Palace Convention Center',
    location: '4th Block, Jayanagar, Bangalore',
    distance: '2.5 km',
    rating: 4.9,
    reviewCount: 128,
    capacity: 1200,
    minCapacity: 800,
    maxCapacity: 1200,
    rooms: 15,
    price: 150000,
    imageUrl: 'https://images.unsplash.com/photo-1528605248644-14dd04022da1?w=800&h=600&fit=crop',
    images: [
      'https://images.unsplash.com/photo-1528605248644-14dd04022da1?w=800&h=600&fit=crop',
      'https://images.unsplash.com/photo-1522335789203-aabd1fc54bc9?w=800&h=600&fit=crop',
      'https://images.unsplash.com/photo-1519741497674-611481863552?w=800&h=600&fit=crop'
    ],
    amenities: ['AC'],
    fullAmenities: [
      'Centralized Air Conditioning',
      'In-house Catering',
      'Sound System',
      'Live Streaming',
      '24/7 Power Backup',
      'Bridal Makeup Room'
    ],
    description: 'Royal Palace Convention Center stands as one of Bangalore\'s most prestigious wedding venues.',
    fullDescription: 'Royal Palace Convention Center stands as one of Bangalore\'s most prestigious wedding venues, blending traditional elegance with modern luxury for grand celebrations. Our venue offers state-of-the-art facilities, professional event management services, and a team dedicated to making your special day unforgettable.',
    parking: '100+ Cars Parking',
    latitude: 12.9716,
    longitude: 77.5946,
    reviews: [
      {
        id: '1',
        author: 'Ananya Rao',
        rating: 5.0,
        comment: 'Stunning venue with excellent management. Everything was perfect!',
        date: '2024-12-15',
        verified: true
      },
      {
        id: '2',
        author: 'Rahul Verma',
        rating: 4.5,
        comment: 'Great location and facilities. Highly recommended for large events.',
        date: '2024-11-20',
        verified: true
      },
      {
        id: '3',
        author: 'Priya Sharma',
        rating: 5.0,
        comment: 'Beautiful venue with professional staff. Made our wedding day perfect!',
        date: '2024-10-10',
        verified: true
      }
    ],
    policies: [
      'Advance booking required',
      'Cancellation policy applies',
      'Security deposit required'
    ],
    cancellationPolicy: 'Full refund if cancelled 30 days before event',
    checkInTime: '10:00 AM',
    checkOutTime: '11:00 PM'
  }
};

export const MOCK_SIMILAR_HALLS: Record<string, HallDto[]> = {
  '1': [
    {
      id: '7',
      name: 'Grand Lotus Venue',
      location: 'Koramangala, Bangalore',
      rating: 4.8,
      reviewCount: 95,
      capacity: 1000,
      rooms: 12,
      price: 225000,
      imageUrl: 'https://images.unsplash.com/photo-1519741497674-611481863552?w=800&h=600&fit=crop',
      amenities: ['AC', 'Valet Parking']
    },
    {
      id: '8',
      name: 'Anugraha Hall',
      location: 'Malleswaram, Bangalore',
      rating: 4.6,
      reviewCount: 78,
      capacity: 600,
      rooms: 8,
      price: 60000,
      imageUrl: 'https://images.unsplash.com/photo-1522335789203-aabd1fc54bc9?w=800&h=600&fit=crop',
      amenities: ['AC']
    },
    {
      id: '9',
      name: 'Silver Oak Banquets',
      location: 'Indiranagar, Bangalore',
      rating: 4.7,
      reviewCount: 102,
      capacity: 700,
      rooms: 9,
      price: 85000,
      imageUrl: 'https://images.unsplash.com/photo-1528605248644-14dd04022da1?w=800&h=600&fit=crop',
      amenities: ['AC', 'Valet Parking']
    },
    {
      id: '10',
      name: 'Imperial Gardens',
      location: 'Whitefield, Bangalore',
      rating: 4.9,
      reviewCount: 156,
      capacity: 2000,
      rooms: 25,
      price: 300000,
      imageUrl: 'https://images.unsplash.com/photo-1519741497674-611481863552?w=800&h=600&fit=crop',
      amenities: ['AC', 'Valet Parking', 'Guest Rooms']
    }
  ]
};

