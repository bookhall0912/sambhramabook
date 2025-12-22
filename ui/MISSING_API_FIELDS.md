# Missing API Fields for Services Endpoint

## Current API Response (`GET /services`)
The API currently returns:
```json
{
  "id": "guid",
  "serviceType": 1,
  "title": "string",
  "description": "string | null",
  "city": "string"
}
```

## Required Fields for UI

### For Hall Card Display (Landing Page & Halls List)
The following fields are **MISSING** and need to be added to the API response:

1. **rating** (number) - Hall rating (e.g., 4.5)
2. **reviewCount** (number) - Number of reviews (e.g., 128)
3. **capacity** (number) - Maximum capacity of the hall
4. **minCapacity** (number, optional) - Minimum capacity
5. **maxCapacity** (number, optional) - Maximum capacity
6. **rooms** (number) - Number of rooms in the hall
7. **price** (number) - Price per event/day
8. **imageUrl** (string) - Main image URL for the hall
9. **images** (string[], optional) - Array of image URLs for gallery
10. **amenities** (string[]) - Array of amenities (e.g., ["AC", "Parking", "WiFi"])
11. **parking** (string, optional) - Parking information (e.g., "100+ Cars Parking")
12. **latitude** (number) - Latitude coordinate of the hall (for distance calculation)
13. **longitude** (number) - Longitude coordinate of the hall (for distance calculation)
14. **location** (string) - Full location string (currently only "city" is available, might need area/address)

### For Hall Detail Page
All of the above plus:
15. **fullAmenities** (string[]) - Complete list of amenities with descriptions
16. **reviews** (Review[]) - Array of review objects with:
    - userId (string)
    - userName (string)
    - rating (number)
    - comment (string)
    - date (string)

## Recommended API Response Structure

```json
{
  "id": "guid",
  "serviceType": 1,
  "title": "string",
  "description": "string | null",
  "city": "string",
  "location": "string", // Full address or location string
  "latitude": 12.9716,
  "longitude": 77.5946,
  "rating": 4.5,
  "reviewCount": 128,
  "capacity": 1200,
  "minCapacity": 800,
  "maxCapacity": 1200,
  "rooms": 15,
  "price": 150000,
  "imageUrl": "https://...",
  "images": ["https://...", "https://..."],
  "amenities": ["AC", "Parking", "WiFi"],
  "parking": "100+ Cars Parking"
}
```

## Notes
- The `distance` field is calculated on the frontend using user's location and hall's latitude/longitude
- If any of these fields are missing, the UI will use mock data as fallback
- The current implementation will show halls with default/empty values for missing fields

