# SambhramaBook API Specification

This document outlines all the API endpoints required for the SambhramaBook application.

**Base URL**: `http://localhost:5160` (development)

---

## 1. Landing Page APIs

### 1.1 Get Popular Halls
Get featured/popular halls to display on the landing page.

**Endpoint**: `GET /api/halls/popular`

**Query Parameters**:
- `limit` (optional, number): Number of halls to return. Default: 3

**Response**:
```json
{
  "halls": [
    {
      "id": "string",
      "name": "string",
      "location": "string",
      "distance": "string (optional)",
      "rating": 4.9,
      "reviewCount": 128,
      "capacity": 1200,
      "minCapacity": 800,
      "maxCapacity": 1200,
      "rooms": 15,
      "price": 150000,
      "imageUrl": "string (URL)",
      "images": ["string (URL array, optional)"],
      "amenities": ["AC", "Valet Parking"],
      "description": "string (optional)",
      "parking": "string (optional)",
      "latitude": 12.9716,
      "longitude": 77.5946
    }
  ],
  "total": 3,
  "page": 1,
  "pageSize": 3,
  "totalPages": 1
}
```

---

## 2. Halls Listing Page APIs

### 2.1 Search/Filter Halls
Search and filter halls with pagination support.

**Endpoint**: `GET /api/halls`

**Query Parameters**:
- `location` (optional, string): Location/city name (e.g., "Bangalore")
- `minPrice` (optional, number): Minimum price per day
- `maxPrice` (optional, number): Maximum price per day
- `minCapacity` (optional, number): Minimum guest capacity
- `maxCapacity` (optional, number): Maximum guest capacity
- `amenities` (optional, string): Comma-separated list (e.g., "AC,Valet Parking,Guest Rooms")
- `page` (optional, number): Page number. Default: 1
- `pageSize` (optional, number): Items per page. Default: 12
- `date` (optional, string): ISO date string (YYYY-MM-DD) for availability check
- `days` (optional, number): Number of days needed
- `guests` (optional, number): Number of guests

**Example Request**:
```
GET /api/halls?location=Bangalore&maxPrice=200000&minCapacity=500&page=1&pageSize=12
```

**Response**:
```json
{
  "halls": [
    {
      "id": "string",
      "name": "string",
      "location": "string",
      "distance": "string (optional)",
      "rating": 4.9,
      "reviewCount": 128,
      "capacity": 1200,
      "minCapacity": 800,
      "maxCapacity": 1200,
      "rooms": 15,
      "price": 150000,
      "imageUrl": "string (URL)",
      "images": ["string (URL array, optional)"],
      "amenities": ["AC", "Valet Parking"],
      "description": "string (optional)",
      "parking": "string (optional)",
      "latitude": 12.9716,
      "longitude": 77.5946
    }
  ],
  "total": 124,
  "page": 1,
  "pageSize": 12,
  "totalPages": 11
}
```

---

## 3. Hall Detail Page APIs

### 3.1 Get Hall Details
Get complete details of a specific hall by ID.

**Endpoint**: `GET /api/halls/{id}`

**Path Parameters**:
- `id` (string): Hall ID

**Response**:
```json
{
  "id": "string",
  "name": "Royal Palace Convention Center",
  "location": "4th Block, Jayanagar, Bangalore",
  "distance": "2.5 km",
  "rating": 4.9,
  "reviewCount": 128,
  "capacity": 1200,
  "minCapacity": 800,
  "maxCapacity": 1200,
  "rooms": 15,
  "price": 150000,
  "imageUrl": "string (URL)",
  "images": [
    "https://images.unsplash.com/photo-1528605248644-14dd04022da1",
    "https://images.unsplash.com/photo-1522335789203-aabd1fc54bc9",
    "https://images.unsplash.com/photo-1519741497674-611481863552"
  ],
  "amenities": ["AC"],
  "fullAmenities": [
    "Centralized Air Conditioning",
    "In-house Catering",
    "Sound System",
    "Live Streaming",
    "24/7 Power Backup",
    "Bridal Makeup Room"
  ],
  "description": "Brief description",
  "fullDescription": "Royal Palace Convention Center stands as one of Bangalore's most prestigious wedding venues, blending traditional elegance with modern luxury for grand celebrations.",
  "parking": "100+ Cars Parking",
  "latitude": 12.9716,
  "longitude": 77.5946,
  "reviews": [
    {
      "id": "string",
      "author": "Ananya Rao",
      "rating": 5.0,
      "comment": "Stunning venue with excellent management.",
      "date": "2024-12-15",
      "verified": true
    }
  ],
  "policies": ["string (optional)"],
  "cancellationPolicy": "string (optional)",
  "checkInTime": "string (optional)",
  "checkOutTime": "string (optional)"
}
```

### 3.2 Get Similar Halls
Get similar/recommended halls based on a hall ID.

**Endpoint**: `GET /api/halls/{id}/similar`

**Path Parameters**:
- `id` (string): Hall ID

**Query Parameters**:
- `limit` (optional, number): Number of similar halls to return. Default: 4

**Response**:
```json
{
  "halls": [
    {
      "id": "string",
      "name": "string",
      "location": "string",
      "rating": 4.8,
      "reviewCount": 95,
      "capacity": 1000,
      "rooms": 12,
      "price": 225000,
      "imageUrl": "string (URL)",
      "amenities": ["AC", "Valet Parking"]
    }
  ],
  "total": 4,
  "page": 1,
  "pageSize": 4,
  "totalPages": 1
}
```

### 3.3 Get Hall Availability
Get availability calendar for a specific hall.

**Endpoint**: `GET /api/halls/{id}/availability`

**Path Parameters**:
- `id` (string): Hall ID

**Query Parameters**:
- `month` (string): Month name (e.g., "October")
- `year` (number): Year (e.g., 2025)

**Response**:
```json
{
  "month": "October",
  "year": 2025,
  "days": [
    {
      "day": 1,
      "status": "booked",
      "price": null
    },
    {
      "day": 2,
      "status": "available",
      "price": 150000
    },
    {
      "day": 3,
      "status": "available",
      "price": 150000
    },
    {
      "day": 10,
      "status": "available",
      "price": 180000
    }
  ]
}
```

**Status Values**:
- `available`: Hall is available for booking
- `booked`: Hall is already booked
- `unavailable`: Hall is not available (maintenance, etc.)

---

## 4. Booking APIs

### 4.1 Submit Booking Request
Submit a booking request for a hall.

**Endpoint**: `POST /api/bookings`

**Request Body**:
```json
{
  "hallId": "string",
  "startDate": "2025-10-10",
  "endDate": "2025-10-10",
  "numberOfDays": 1,
  "guests": 800,
  "contactInfo": {
    "name": "John Doe",
    "email": "john.doe@example.com",
    "phone": "+91 98765 43210"
  },
  "specialRequests": "string (optional)"
}
```

**Response**:
```json
{
  "bookingId": "string (UUID)",
  "hallId": "string",
  "hallName": "Royal Palace Convention Center",
  "startDate": "2025-10-10",
  "endDate": "2025-10-10",
  "numberOfDays": 1,
  "guests": 800,
  "totalAmount": 179700,
  "status": "pending",
  "confirmationNumber": "SB-2025-001234",
  "message": "Your booking request has been submitted. We will confirm shortly."
}
```

**Status Values**:
- `pending`: Booking request is pending confirmation
- `confirmed`: Booking is confirmed
- `cancelled`: Booking is cancelled

---

## Error Responses

All endpoints may return the following error responses:

**400 Bad Request**:
```json
{
  "error": "Validation failed",
  "message": "Invalid request parameters",
  "details": {}
}
```

**404 Not Found**:
```json
{
  "error": "Not Found",
  "message": "Hall not found"
}
```

**500 Internal Server Error**:
```json
{
  "error": "Internal Server Error",
  "message": "An unexpected error occurred"
}
```

---

## Notes

1. All dates should be in ISO 8601 format (YYYY-MM-DD)
2. Prices are in Indian Rupees (INR)
3. Ratings are on a scale of 0-5
4. All image URLs should be absolute URLs
5. Pagination: `page` starts from 1
6. Distance is calculated from a reference point (can be user's location or city center)

