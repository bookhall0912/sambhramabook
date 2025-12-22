import { ServiceCategoryDto } from '../models/dtos/service-category.dto';

/**
 * Mock data for development purposes.
 * TODO: Remove this file once backend APIs are fully implemented.
 */

export const MOCK_SERVICE_CATEGORIES: ServiceCategoryDto[] = [
  {
    code: 'catering',
    displayName: 'Catering Services',
    description: 'Delight your guests with authentic cuisines from top-rated caterers. From traditional South Indian to continental, we offer a wide range of culinary options.',
    iconUrl: 'https://images.unsplash.com/photo-1556910103-1c02745aae4d?w=200&h=200&fit=crop',
    backgroundImageUrl: 'https://images.unsplash.com/photo-1556910103-1c02745aae4d?w=1200&h=600&fit=crop',
    themeColor: '#FF6B35',
    displayOrder: 1
  },
  {
    code: 'photography',
    displayName: 'Photography & Videography',
    description: 'Capture every precious moment with our verified professional photographers. High-quality photos and videos to preserve your special day forever.',
    iconUrl: 'https://images.unsplash.com/photo-1492691527719-9d1e07e534b4?w=200&h=200&fit=crop',
    backgroundImageUrl: 'https://images.unsplash.com/photo-1492691527719-9d1e07e534b4?w=1200&h=600&fit=crop',
    themeColor: '#4A90E2',
    displayOrder: 2
  },
  {
    code: 'decoration',
    displayName: 'Decoration & Styling',
    description: 'Transform your venue with beautiful decorations. From elegant floral arrangements to stunning stage setups, we bring your vision to life.',
    iconUrl: 'https://images.unsplash.com/photo-1519167758481-83f550bb49b3?w=200&h=200&fit=crop',
    backgroundImageUrl: 'https://images.unsplash.com/photo-1519167758481-83f550bb49b3?w=1200&h=600&fit=crop',
    themeColor: '#9B59B6',
    displayOrder: 3
  },
  {
    code: 'entertainment',
    displayName: 'Entertainment',
    description: 'DJ, live music, and entertainment services to keep your guests engaged. Professional sound systems and lighting for an unforgettable experience.',
    iconUrl: 'https://images.unsplash.com/photo-1470229722913-7c0e2dbbafd3?w=200&h=200&fit=crop',
    backgroundImageUrl: 'https://images.unsplash.com/photo-1470229722913-7c0e2dbbafd3?w=1200&h=600&fit=crop',
    themeColor: '#E74C3C',
    displayOrder: 4
  },
  {
    code: 'planning',
    displayName: 'Event Planning',
    description: 'End-to-end planning and execution for a stress-free celebration. Our expert event planners handle every detail so you can enjoy your special day.',
    iconUrl: 'https://images.unsplash.com/photo-1511578314322-379afb476865?w=200&h=200&fit=crop',
    backgroundImageUrl: 'https://images.unsplash.com/photo-1511578314322-379afb476865?w=1200&h=600&fit=crop',
    themeColor: '#16A085',
    displayOrder: 5
  }
];

