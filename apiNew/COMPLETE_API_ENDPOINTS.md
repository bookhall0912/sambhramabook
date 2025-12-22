# Complete API Endpoints with Request/Response Contracts

## Base URL: `/api`

---

## 1. Authentication APIs

### 1.1 Send OTP
**POST** `/api/auth/send-otp`

**Request:**
```json
{
  "mobileOrEmail": "9876543210" // or "user@example.com"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "otpSent": true,
    "message": "OTP sent successfully",
    "expiresIn": 300 // seconds
  }
}
```

---

### 1.2 Verify OTP & Login
**POST** `/api/auth/verify-otp`

**Request:**
```json
{
  "mobileOrEmail": "9876543210",
  "otp": "123456"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "refresh_token_here",
    "user": {
      "id": "user-123",
      "name": "User Name",
      "email": "user@example.com",
      "mobile": "9876543210",
      "role": "User" // "User" | "Vendor" | "Admin"
    }
  }
}
```

---

### 1.3 Get Current User
**GET** `/api/auth/me`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "user-123",
    "name": "User Name",
    "email": "user@example.com",
    "mobile": "9876543210",
    "role": "User",
    "vendorProfileComplete": false // only for vendors
  }
}
```

---

### 1.4 Logout
**POST** `/api/auth/logout`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "message": "Logged out successfully"
}
```

---

### 1.5 Refresh Token
**POST** `/api/auth/refresh-token`

**Request:**
```json
{
  "refreshToken": "refresh_token_here"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "token": "new_access_token",
    "refreshToken": "new_refresh_token"
  }
}
```

---

## 2. Vendor Onboarding APIs

### 2.1 Complete Onboarding
**POST** `/api/vendor/onboarding`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "businessName": "Royal Grand Hall",
  "businessType": "Hall Owner", // "Hall Owner" | "Service Provider"
  "businessEmail": "business@example.com",
  "businessPhone": "9876543210",
  "address": "123 Main Street",
  "city": "Bangalore",
  "state": "Karnataka",
  "pincode": "560041",
  "gstNumber": "29ABCDE1234F1Z5", // optional
  "panNumber": "ABCDE1234F", // optional
  "bankAccountNumber": "1234567890", // optional
  "ifscCode": "BANK0001234" // optional
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "vendorId": "vendor-123",
    "onboardingComplete": true,
    "message": "Onboarding completed successfully"
  }
}
```

---

### 2.2 Get Onboarding Status
**GET** `/api/vendor/onboarding/status`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "onboardingComplete": false,
    "completedSteps": ["business_info", "location"],
    "pendingSteps": ["additional_details"]
  }
}
```

---

### 2.3 Update Onboarding
**PUT** `/api/vendor/onboarding`

**Headers:** `Authorization: Bearer {token}`

**Request:** Same as POST `/api/vendor/onboarding`

**Response:**
```json
{
  "success": true,
  "data": {
    "vendorId": "vendor-123",
    "message": "Onboarding updated successfully"
  }
}
```

---

## 3. Landing Page APIs

### 3.1 Get Popular Halls
**GET** `/api/halls/popular?limit=3`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "hall-1",
      "name": "Royal Grand Hall",
      "location": "JP Nagar, Bangalore",
      "distance": "2.5 km",
      "rating": 4.8,
      "reviewCount": 120,
      "capacity": 1000,
      "minCapacity": 200,
      "maxCapacity": 1000,
      "rooms": 5,
      "price": 150000,
      "imageUrl": "https://example.com/image.jpg",
      "amenities": ["AC", "Parking", "Power Backup"]
    }
  ]
}
```

---

### 3.2 Get Service Categories
**GET** `/api/services/categories`

**Response:**
```json
{
  "success": true,
  "data": {
    "categories": [
      {
        "code": "photography",
        "displayName": "Photography",
        "description": "Professional wedding photographers",
        "iconUrl": "https://example.com/icon.png",
        "backgroundImageUrl": "https://example.com/bg.jpg",
        "themeColor": "#FF6B6B",
        "displayOrder": 1
      }
    ]
  }
}
```

---

## 4. Halls Listing & Search APIs

### 4.1 Search & Filter Halls
**GET** `/api/halls?location=Bangalore&minPrice=50000&maxPrice=200000&minCapacity=200&maxCapacity=1000&amenities=AC,Parking&date=2024-12-25&days=2&guests=500&page=1&pageSize=12`

**Query Parameters:**
- `location` (string, optional)
- `minPrice` (number, optional)
- `maxPrice` (number, optional)
- `minCapacity` (number, optional)
- `maxCapacity` (number, optional)
- `amenities` (string, optional, comma-separated)
- `date` (string, optional, ISO date)
- `days` (number, optional)
- `guests` (number, optional)
- `page` (number, default: 1)
- `pageSize` (number, default: 12)

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "hall-1",
      "name": "Royal Grand Hall",
      "location": "JP Nagar, Bangalore",
      "distance": "2.5 km",
      "rating": 4.8,
      "reviewCount": 120,
      "capacity": 1000,
      "minCapacity": 200,
      "maxCapacity": 1000,
      "rooms": 5,
      "price": 150000,
      "imageUrl": "https://example.com/image.jpg",
      "images": ["https://example.com/img1.jpg", "https://example.com/img2.jpg"],
      "amenities": ["AC", "Parking", "Power Backup"],
      "description": "A grand convention hall",
      "parking": "Ample parking available",
      "latitude": 12.9716,
      "longitude": 77.5946
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 12,
    "total": 45,
    "totalPages": 4
  }
}
```

---

### 4.2 Get Hall Details by ID
**GET** `/api/halls/{id}`

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "hall-1",
    "name": "Royal Grand Hall",
    "location": "JP Nagar, Bangalore",
    "distance": "2.5 km",
    "rating": 4.8,
    "reviewCount": 120,
    "capacity": 1000,
    "minCapacity": 200,
    "maxCapacity": 1000,
    "rooms": 5,
    "price": 150000,
    "imageUrl": "https://example.com/image.jpg",
    "images": ["https://example.com/img1.jpg"],
    "amenities": ["AC", "Parking"],
    "fullDescription": "A grand convention hall perfect for weddings...",
    "fullAmenities": ["Centralized Air Conditioning", "Ample Parking"],
    "parking": "Ample parking available",
    "policies": ["Advance booking required"],
    "cancellationPolicy": "Standard cancellation policy applies",
    "checkInTime": "10:00 AM",
    "checkOutTime": "11:00 PM",
    "reviews": [
      {
        "id": "review-1",
        "author": "John Doe",
        "rating": 5,
        "comment": "Excellent venue!",
        "date": "2024-10-15",
        "verified": true
      }
    ]
  }
}
```

---

### 4.3 Get Hall Details by Slug
**GET** `/api/halls/slug/{slug}`

**Response:** Same as 4.2

---

### 4.4 Get Hall Availability
**GET** `/api/halls/{id}/availability?month=12&year=2024`

**Response:**
```json
{
  "success": true,
  "data": {
    "hallId": "hall-1",
    "month": 12,
    "year": 2024,
    "days": [
      {
        "day": 1,
        "status": "available" // "available" | "booked" | "unavailable"
      },
      {
        "day": 2,
        "status": "booked"
      }
    ]
  }
}
```

---

### 4.5 Get Hall Reviews
**GET** `/api/halls/{id}/reviews?page=1&pageSize=10`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "review-1",
      "author": "John Doe",
      "rating": 5,
      "comment": "Excellent venue!",
      "date": "2024-10-15",
      "verified": true
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 10,
    "total": 25,
    "totalPages": 3
  }
}
```

---

## 5. Services APIs

### 5.1 Get Services by Type
**GET** `/api/services/{type}?location=Bangalore&page=1&pageSize=12`

**Path Parameters:**
- `type` (string): photography, catering, decoration, etc.

**Query Parameters:**
- `location` (string, optional)
- `page` (number, default: 1)
- `pageSize` (number, default: 12)

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "service-1",
      "serviceType": 2, // 1=Hall, 2=Photography, etc.
      "title": "Lens & Light Studios",
      "description": "Professional wedding photography",
      "city": "Bangalore",
      "location": "Indiranagar, Bangalore",
      "latitude": 12.9784,
      "longitude": 77.6408,
      "rating": 4.9,
      "reviewCount": 120,
      "price": 50000,
      "imageUrl": "https://example.com/image.jpg",
      "images": ["https://example.com/img1.jpg"]
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 12,
    "total": 30,
    "totalPages": 3
  }
}
```

---

### 5.2 Get Service Vendor Details
**GET** `/api/services/{type}/{id}`

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "service-1",
    "serviceType": 2,
    "title": "Lens & Light Studios",
    "description": "Professional wedding photography",
    "city": "Bangalore",
    "location": "Indiranagar, Bangalore",
    "latitude": 12.9784,
    "longitude": 77.6408,
    "rating": 4.9,
    "reviewCount": 120,
    "price": 50000,
    "imageUrl": "https://example.com/image.jpg",
    "images": ["https://example.com/img1.jpg"],
    "vendorName": "Lens & Light Studios",
    "vendorImage": "https://example.com/vendor.jpg",
    "vendorLocation": "Indiranagar, Bangalore",
    "vendorRating": 4.9,
    "vendorReviewCount": 120,
    "vendorExperience": 5,
    "isVerified": true,
    "about": "Lens & Light Studios is a premier wedding photography team...",
    "specialities": ["Candid Photography", "Cinematic Video"],
    "packages": [
      {
        "id": "pkg-1",
        "name": "Standard Package",
        "price": 50000,
        "priceUnit": "/ day",
        "features": ["1 Candid Photographer", "1 Traditional Photographer"],
        "isPopular": false
      }
    ],
    "portfolioImages": ["https://example.com/portfolio1.jpg"],
    "reviews": [
      {
        "id": "review-1",
        "author": "John Doe",
        "rating": 5,
        "comment": "Excellent service!",
        "date": "2024-10-15",
        "verified": true
      }
    ]
  }
}
```

---

### 5.3 Get Service Packages
**GET** `/api/services/{type}/{id}/packages`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "pkg-1",
      "name": "Standard Package",
      "price": 50000,
      "priceUnit": "/ day",
      "features": ["1 Candid Photographer", "1 Traditional Photographer"],
      "isPopular": false
    }
  ]
}
```

---

### 5.4 Get Service Portfolio
**GET** `/api/services/{type}/{id}/portfolio`

**Response:**
```json
{
  "success": true,
  "data": {
    "images": [
      "https://example.com/portfolio1.jpg",
      "https://example.com/portfolio2.jpg"
    ]
  }
}
```

---

### 5.5 Get Service Reviews
**GET** `/api/services/{type}/{id}/reviews?page=1&pageSize=10`

**Response:** Same as 4.5

---

## 6. Booking APIs

### 6.1 Create Booking
**POST** `/api/bookings`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "hallId": "hall-1", // or "serviceId" for services
  "startDate": "2024-12-25",
  "endDate": "2024-12-26",
  "numberOfDays": 2,
  "guests": 500,
  "contactInfo": {
    "name": "John Doe",
    "email": "john@example.com",
    "phone": "9876543210"
  },
  "specialRequests": "Need extra parking"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "bookingId": "SB-8824-X901",
    "hallId": "hall-1",
    "hallName": "Royal Grand Hall",
    "startDate": "2024-12-25",
    "endDate": "2024-12-26",
    "numberOfDays": 2,
    "guests": 500,
    "totalAmount": 306425,
    "status": "pending", // "pending" | "confirmed" | "cancelled"
    "confirmationNumber": "SB-8824-X901",
    "message": "Booking submitted successfully"
  }
}
```

---

### 6.2 Get Booking Details
**GET** `/api/bookings/{id}`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "booking-1",
    "bookingId": "SB-8824-X901",
    "referenceId": "SB-8824-X901",
    "venueName": "Royal Grand Hall",
    "venueImage": "https://example.com/image.jpg",
    "location": "JP Nagar, Bangalore",
    "startDate": "2024-12-25",
    "endDate": "2024-12-26",
    "days": 2,
    "guestCount": 500,
    "totalAmount": 306425,
    "status": "UPCOMING", // "UPCOMING" | "PAST" | "CANCELLED" | "PENDING_CONFIRMATION"
    "paymentStatus": "PAID", // "PAID" | "PENDING" | "REFUNDED"
    "eventType": "Wedding Reception",
    "contactInfo": {
      "name": "John Doe",
      "email": "john@example.com",
      "phone": "9876543210"
    },
    "specialRequests": "Need extra parking"
  }
}
```

---

### 6.3 Get Booking by Reference
**GET** `/api/bookings/reference/{reference}`

**Headers:** `Authorization: Bearer {token}`

**Response:** Same as 6.2

---

### 6.4 Cancel Booking
**PUT** `/api/bookings/{id}/cancel`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "reason": "Change of plans"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "bookingId": "SB-8824-X901",
    "status": "CANCELLED",
    "refundAmount": 153212.5,
    "message": "Booking cancelled successfully"
  }
}
```

---

### 6.5 Reschedule Booking
**PUT** `/api/bookings/{id}/reschedule`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "newStartDate": "2025-01-10",
  "newEndDate": "2025-01-11",
  "reason": "Date change requested"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "bookingId": "SB-8824-X901",
    "startDate": "2025-01-10",
    "endDate": "2025-01-11",
    "message": "Reschedule request submitted"
  }
}
```

---

### 6.6 Process Payment
**POST** `/api/bookings/{id}/payment`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "paymentMethod": "razorpay", // "razorpay" | "stripe" | "cash"
  "amount": 306425
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "paymentId": "pay_123",
    "orderId": "order_123",
    "amount": 306425,
    "status": "pending",
    "paymentUrl": "https://checkout.razorpay.com/..."
  }
}
```

---

## 7. User Bookings APIs

### 7.1 Get User Bookings
**GET** `/api/user/bookings?status=UPCOMING&page=1&pageSize=10`

**Headers:** `Authorization: Bearer {token}`

**Query Parameters:**
- `status` (string, optional): UPCOMING | PAST | CANCELLED
- `page` (number, default: 1)
- `pageSize` (number, default: 10)

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "booking-1",
      "referenceId": "SB-8824-X901",
      "venueName": "Royal Grand Hall",
      "venueImage": "https://example.com/image.jpg",
      "location": "JP Nagar, Bangalore",
      "startDate": "2024-12-25",
      "endDate": "2024-12-26",
      "days": 2,
      "guestCount": 500,
      "totalAmount": 306425,
      "status": "UPCOMING",
      "paymentStatus": "PAID",
      "eventType": "Wedding Reception"
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 10,
    "total": 5,
    "totalPages": 1
  }
}
```

---

### 7.2 Get Specific User Booking
**GET** `/api/user/bookings/{id}`

**Headers:** `Authorization: Bearer {token}`

**Response:** Same as 6.2

---

## 8. Vendor Bookings APIs

### 8.1 Get Vendor Bookings
**GET** `/api/vendor/bookings?status=all&page=1&pageSize=10`

**Headers:** `Authorization: Bearer {token}`

**Query Parameters:**
- `status` (string, optional): all | pending | confirmed
- `page` (number, default: 1)
- `pageSize` (number, default: 10)

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "booking-1",
      "bookingId": "SB-9012",
      "customerName": "Amit Verma",
      "customerPhone": "+91 98765 43210",
      "venueName": "Royal Grand Hall",
      "date": "2024-11-12",
      "days": 2,
      "guests": 500,
      "amount": 300000,
      "status": "CONFIRMED" // "CONFIRMED" | "PENDING" | "CANCELLED"
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 10,
    "total": 15,
    "totalPages": 2
  }
}
```

---

### 8.2 Approve Booking
**PUT** `/api/vendor/bookings/{id}/approve`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "bookingId": "SB-9012",
    "status": "CONFIRMED",
    "message": "Booking approved successfully"
  }
}
```

---

### 8.3 Reject Booking
**PUT** `/api/vendor/bookings/{id}/reject`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "reason": "Date not available"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "bookingId": "SB-9012",
    "status": "CANCELLED",
    "message": "Booking rejected"
  }
}
```

---

### 8.4 Create Booking Manually (Vendor)
**POST** `/api/vendor/bookings`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "listingId": "hall-1",
  "customerName": "John Doe",
  "customerPhone": "9876543210",
  "customerEmail": "john@example.com",
  "startDate": "2024-12-25",
  "endDate": "2024-12-26",
  "guests": 500,
  "amount": 300000
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "bookingId": "SB-9012",
    "status": "CONFIRMED",
    "message": "Booking created successfully"
  }
}
```

---

## 9. Payment APIs

### 9.1 Initiate Payment
**POST** `/api/payments/initiate`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "bookingId": "SB-8824-X901",
  "amount": 306425,
  "paymentMethod": "razorpay"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "paymentId": "pay_123",
    "orderId": "order_123",
    "amount": 306425,
    "paymentUrl": "https://checkout.razorpay.com/...",
    "status": "pending"
  }
}
```

---

### 9.2 Verify Payment
**POST** `/api/payments/verify`

**Request:**
```json
{
  "paymentId": "pay_123",
  "orderId": "order_123",
  "signature": "signature_from_razorpay"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "paymentId": "pay_123",
    "status": "success",
    "amount": 306425,
    "bookingId": "SB-8824-X901"
  }
}
```

---

### 9.3 Process Refund
**POST** `/api/payments/{id}/refund`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "amount": 153212.5, // optional, full refund if not provided
  "reason": "Booking cancelled"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "refundId": "refund_123",
    "amount": 153212.5,
    "status": "processed"
  }
}
```

---

## 10. User Profile APIs

### 10.1 Get User Profile
**GET** `/api/user/profile`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "user-123",
    "name": "John Doe",
    "email": "john@example.com",
    "mobile": "9876543210",
    "address": "123 Main St",
    "city": "Bangalore",
    "state": "Karnataka",
    "pincode": "560041"
  }
}
```

---

### 10.2 Update User Profile
**PUT** `/api/user/profile`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "fullName": "John Doe",
  "email": "john@example.com",
  "mobile": "9876543210",
  "address": "123 Main St",
  "city": "Bangalore",
  "state": "Karnataka",
  "pincode": "560041"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "user-123",
    "message": "Profile updated successfully"
  }
}
```

---

### 10.3 Change Password
**PUT** `/api/user/profile/password`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "currentPassword": "oldPassword123",
  "newPassword": "newPassword123"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Password changed successfully"
}
```

---

## 11. Saved Venues APIs

### 11.1 Get Saved Listings
**GET** `/api/user/saved`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "hall-1",
      "name": "Royal Orchid Hall",
      "image": "https://example.com/image.jpg",
      "location": "Indiranagar, Bangalore",
      "rating": 4.9,
      "reviewCount": 120
    }
  ]
}
```

---

### 11.2 Save Listing
**POST** `/api/user/saved/{listingId}`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "message": "Listing saved successfully"
}
```

---

### 11.3 Remove Saved Listing
**DELETE** `/api/user/saved/{listingId}`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "message": "Listing removed from saved"
}
```

---

## 12. Notifications APIs

### 12.1 Get Notifications
**GET** `/api/user/notifications?page=1&pageSize=20`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "notif-1",
      "title": "Booking Confirmed",
      "message": "Your booking for Sambhrama Grand Hall has been confirmed.",
      "type": "booking", // "booking" | "payment" | "reminder" | "system"
      "read": false,
      "timestamp": "2024-11-10T10:30:00Z",
      "actionUrl": "/booking/confirmation?bookingId=SB-8824-X901"
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 20,
    "total": 15,
    "totalPages": 1
  }
}
```

---

### 12.2 Mark Notification as Read
**PUT** `/api/user/notifications/{id}/read`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "message": "Notification marked as read"
}
```

---

### 12.3 Mark All Notifications as Read
**PUT** `/api/user/notifications/read-all`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "message": "All notifications marked as read"
}
```

---

### 12.4 Get Unread Count
**GET** `/api/user/notifications/unread-count`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "unreadCount": 5
  }
}
```

---

## 13. Vendor Dashboard APIs

### 13.1 Get Dashboard Overview
**GET** `/api/vendor/dashboard/overview`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "totalEarnings": 452000,
    "upcomingEvents": 8,
    "profileViews": 1204,
    "earningsChange": 12.5,
    "profileViewsChange": 5,
    "nextEvent": "Priya Weds Rahul (12 Nov)",
    "recentBookings": [
      {
        "id": "1",
        "bookingId": "SB-9012",
        "customerName": "Amit Verma",
        "date": "2024-11-12",
        "status": "CONFIRMED"
      }
    ],
    "listings": [
      {
        "id": "1",
        "name": "Royal Grand Hall",
        "image": "https://example.com/image.jpg",
        "location": "Bangalore",
        "status": "ACTIVE" // "ACTIVE" | "DRAFT" | "INACTIVE"
      }
    ],
    "pendingBookingsCount": 1
  }
}
```

---

### 13.2 Get Recent Bookings
**GET** `/api/vendor/dashboard/recent-bookings?limit=5`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "1",
      "bookingId": "SB-9012",
      "customerName": "Amit Verma",
      "date": "2024-11-12",
      "status": "CONFIRMED"
    }
  ]
}
```

---

### 13.3 Get Listings Summary
**GET** `/api/vendor/dashboard/listings`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "1",
      "name": "Royal Grand Hall",
      "image": "https://example.com/image.jpg",
      "location": "Bangalore",
      "status": "ACTIVE"
    }
  ]
}
```

---

## 14. Vendor Listings APIs

### 14.1 Get All Vendor Listings
**GET** `/api/vendor/listings?page=1&pageSize=10`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "listing-1",
      "name": "Royal Grand Hall",
      "image": "https://example.com/image.jpg",
      "location": "Bangalore",
      "status": "ACTIVE", // "ACTIVE" | "DRAFT" | "INACTIVE"
      "type": "Hall" // "Hall" | "Service"
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 10,
    "total": 5,
    "totalPages": 1
  }
}
```

---

### 14.2 Get Listing Details
**GET** `/api/vendor/listings/{id}`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "listing-1",
    "name": "Royal Grand Hall",
    "hallType": "Convention Hall",
    "yearEstablished": "2015",
    "description": "A grand convention hall perfect for weddings and events.",
    "addressLine1": "123 Main Street",
    "area": "Jayanagar",
    "city": "Bangalore",
    "pincode": "560041",
    "floatingCapacity": 1000,
    "diningCapacity": 400,
    "pricePerDay": 150000,
    "advanceAmount": 50,
    "cancellationPolicy": "No refund if cancelled within 30 days",
    "amenities": [
      {
        "id": "ac",
        "name": "Air Conditioning",
        "selected": true
      }
    ],
    "images": ["https://example.com/img1.jpg"],
    "status": "ACTIVE"
  }
}
```

---

### 14.3 Create Listing
**POST** `/api/vendor/listings`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "name": "Royal Grand Hall",
  "hallType": "Convention Hall",
  "yearEstablished": "2015",
  "description": "A grand convention hall",
  "addressLine1": "123 Main Street",
  "area": "Jayanagar",
  "city": "Bangalore",
  "pincode": "560041",
  "floatingCapacity": 1000,
  "diningCapacity": 400,
  "pricePerDay": 150000,
  "advanceAmount": 50,
  "cancellationPolicy": "No refund if cancelled within 30 days",
  "amenities": ["ac", "parking"],
  "images": ["https://example.com/img1.jpg"],
  "status": "DRAFT" // "DRAFT" | "ACTIVE"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "listing-1",
    "message": "Listing created successfully"
  }
}
```

---

### 14.4 Update Listing
**PUT** `/api/vendor/listings/{id}`

**Headers:** `Authorization: Bearer {token}`

**Request:** Same as 14.3

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "listing-1",
    "message": "Listing updated successfully"
  }
}
```

---

### 14.5 Delete Listing
**DELETE** `/api/vendor/listings/{id}`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "message": "Listing deleted successfully"
}
```

---

### 14.6 Upload Listing Images
**POST** `/api/vendor/listings/{id}/images`

**Headers:** `Authorization: Bearer {token}`

**Request:** `multipart/form-data`
- `images` (file[]): Array of image files

**Response:**
```json
{
  "success": true,
  "data": {
    "images": [
      "https://example.com/uploaded-img1.jpg",
      "https://example.com/uploaded-img2.jpg"
    ]
  }
}
```

---

### 14.7 Update Listing Status
**PUT** `/api/vendor/listings/{id}/status`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "status": "ACTIVE" // "DRAFT" | "ACTIVE" | "INACTIVE"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "listing-1",
    "status": "ACTIVE",
    "message": "Status updated successfully"
  }
}
```

---

## 15. Vendor Availability APIs

### 15.1 Get Availability
**GET** `/api/vendor/availability/{listingId}?month=12&year=2024`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "listingId": "listing-1",
    "month": 12,
    "year": 2024,
    "days": [
      {
        "day": 1,
        "status": "available" // "available" | "booked" | "blocked"
      }
    ]
  }
}
```

---

### 15.2 Update Availability (Bulk)
**PUT** `/api/vendor/availability/{listingId}`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "dates": [
    {
      "date": "2024-12-25",
      "status": "available"
    },
    {
      "date": "2024-12-26",
      "status": "booked"
    }
  ]
}
```

**Response:**
```json
{
  "success": true,
  "message": "Availability updated successfully"
}
```

---

### 15.3 Block Dates
**POST** `/api/vendor/availability/{listingId}/block`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "dates": ["2024-12-25", "2024-12-26"],
  "reason": "Maintenance"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Dates blocked successfully"
}
```

---

### 15.4 Unblock Dates
**DELETE** `/api/vendor/availability/{listingId}/unblock`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "dates": ["2024-12-25", "2024-12-26"]
}
```

**Response:**
```json
{
  "success": true,
  "message": "Dates unblocked successfully"
}
```

---

## 16. Vendor Earnings APIs

### 16.1 Get Earnings Summary
**GET** `/api/vendor/earnings?startDate=2024-01-01&endDate=2024-12-31`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "totalEarnings": 452000,
    "thisMonth": 125000,
    "lastMonth": 98000,
    "pendingPayouts": 50000,
    "totalTransactions": 25
  }
}
```

---

### 16.2 Get Earnings Transactions
**GET** `/api/vendor/earnings/transactions?page=1&pageSize=10`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "txn-1",
      "bookingId": "SB-9012",
      "amount": 300000,
      "commission": 15000,
      "netAmount": 285000,
      "date": "2024-11-12",
      "status": "completed"
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 10,
    "total": 25,
    "totalPages": 3
  }
}
```

---

### 16.3 Get Payout History
**GET** `/api/vendor/earnings/payouts?page=1&pageSize=10`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "payout-1",
      "amount": 50000,
      "date": "2024-11-01",
      "status": "completed",
      "transactionId": "TXN123456"
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 10,
    "total": 5,
    "totalPages": 1
  }
}
```

---

## 17. Vendor Settings APIs

### 17.1 Get Vendor Settings
**GET** `/api/vendor/settings`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "notifications": {
      "email": true,
      "sms": false,
      "push": true
    },
    "bookingSettings": {
      "autoApprove": false,
      "requireDeposit": true
    }
  }
}
```

---

### 17.2 Update Vendor Settings
**PUT** `/api/vendor/settings`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "notifications": {
    "email": true,
    "sms": false,
    "push": true
  },
  "bookingSettings": {
    "autoApprove": false,
    "requireDeposit": true
  }
}
```

**Response:**
```json
{
  "success": true,
  "message": "Settings updated successfully"
}
```

---

## 18. Admin Dashboard APIs

### 18.1 Get Dashboard Overview
**GET** `/api/admin/dashboard/overview`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "totalUsers": 24582,
    "activeVendors": 1240,
    "totalBookings": 8432,
    "platformRevenue": 42500000,
    "usersChange": 12,
    "vendorsChange": 8,
    "bookingsChange": 24,
    "revenueChange": 18,
    "pendingListings": [
      {
        "id": "listing-1",
        "name": "Golden Palms Convention Hall",
        "location": "JP Nagar, Bangalore",
        "vendorName": "Ramesh G",
        "vendorAvatar": "https://example.com/avatar.jpg",
        "type": "Hall",
        "submitted": "2 hours ago",
        "status": "PENDING"
      }
    ],
    "pendingCount": 5,
    "payoutsCount": 2
  }
}
```

---

### 18.2 Get Pending Listings
**GET** `/api/admin/dashboard/pending-listings?limit=5`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "listing-1",
      "name": "Golden Palms Convention Hall",
      "location": "JP Nagar, Bangalore",
      "vendorName": "Ramesh G",
      "vendorAvatar": "https://example.com/avatar.jpg",
      "type": "Hall",
      "submitted": "2 hours ago",
      "status": "PENDING"
    }
  ]
}
```

---

### 18.3 Get Pending Payouts
**GET** `/api/admin/dashboard/pending-payouts?limit=5`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "payout-1",
      "vendorName": "Ramesh G",
      "amount": 50000,
      "requestDate": "2024-11-10"
    }
  ]
}
```

---

## 19. Admin Analytics APIs

### 19.1 Get Analytics Summary
**GET** `/api/admin/analytics/summary?startDate=2024-01-01&endDate=2024-12-31`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "totalRevenue": 42500000,
    "totalBookings": 8432,
    "totalUsers": 24582,
    "totalVendors": 1240,
    "averageBookingValue": 5042
  }
}
```

---

### 19.2 Get Revenue Analytics
**GET** `/api/admin/analytics/revenue?period=monthly&startDate=2024-01-01&endDate=2024-12-31`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "period": "monthly",
    "data": [
      {
        "month": "2024-01",
        "revenue": 3500000,
        "bookings": 650
      }
    ]
  }
}
```

---

### 19.3 Get User Analytics
**GET** `/api/admin/analytics/users?period=monthly`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "period": "monthly",
    "data": [
      {
        "month": "2024-01",
        "newUsers": 250,
        "activeUsers": 1200
      }
    ]
  }
}
```

---

### 19.4 Get Booking Analytics
**GET** `/api/admin/analytics/bookings?period=monthly`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "period": "monthly",
    "data": [
      {
        "month": "2024-01",
        "totalBookings": 650,
        "confirmed": 600,
        "cancelled": 50
      }
    ]
  }
}
```

---

## 20. Admin Users APIs

### 20.1 Get All Users
**GET** `/api/admin/users?page=1&pageSize=10&search=john&status=ACTIVE`

**Headers:** `Authorization: Bearer {token}`

**Query Parameters:**
- `page` (number, default: 1)
- `pageSize` (number, default: 10)
- `search` (string, optional)
- `status` (string, optional): ACTIVE | INACTIVE | SUSPENDED

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "user-1",
      "name": "Aditi Sharma",
      "email": "aditi@example.com",
      "mobile": "+91 98765 43210",
      "avatar": "https://example.com/avatar.jpg",
      "status": "ACTIVE",
      "joinDate": "2024-01-15",
      "bookings": 12
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 10,
    "total": 24582,
    "totalPages": 2459
  }
}
```

---

### 20.2 Get User Details
**GET** `/api/admin/users/{id}`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "user-1",
    "name": "Aditi Sharma",
    "email": "aditi@example.com",
    "mobile": "+91 98765 43210",
    "avatar": "https://example.com/avatar.jpg",
    "status": "ACTIVE",
    "joinDate": "2024-01-15",
    "bookings": 12,
    "totalSpent": 500000,
    "lastLogin": "2024-11-10T10:30:00Z"
  }
}
```

---

### 20.3 Update User Status
**PUT** `/api/admin/users/{id}/status`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "status": "SUSPENDED" // "ACTIVE" | "INACTIVE" | "SUSPENDED"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "user-1",
    "status": "SUSPENDED",
    "message": "User status updated"
  }
}
```

---

### 20.4 Delete User
**DELETE** `/api/admin/users/{id}`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "message": "User deleted successfully"
}
```

---

## 21. Admin Vendors APIs

### 21.1 Get All Vendors
**GET** `/api/admin/vendors?page=1&pageSize=10&status=ACTIVE`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "vendor-1",
      "name": "Ramesh G",
      "email": "ramesh@example.com",
      "mobile": "+91 98765 43210",
      "avatar": "https://example.com/avatar.jpg",
      "status": "ACTIVE",
      "joinDate": "2024-01-10",
      "listings": 5,
      "earnings": 2500000,
      "verificationStatus": "VERIFIED"
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 10,
    "total": 1240,
    "totalPages": 124
  }
}
```

---

### 21.2 Get Vendor Details
**GET** `/api/admin/vendors/{id}`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "vendor-1",
    "name": "Ramesh G",
    "email": "ramesh@example.com",
    "mobile": "+91 98765 43210",
    "avatar": "https://example.com/avatar.jpg",
    "status": "ACTIVE",
    "joinDate": "2024-01-10",
    "listings": 5,
    "earnings": 2500000,
    "verificationStatus": "VERIFIED",
    "businessName": "Royal Grand Hall",
    "businessType": "Hall Owner"
  }
}
```

---

### 21.3 Verify/Reject Vendor
**PUT** `/api/admin/vendors/{id}/verify`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "verificationStatus": "VERIFIED", // "VERIFIED" | "REJECTED"
  "notes": "All documents verified"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "vendor-1",
    "verificationStatus": "VERIFIED",
    "message": "Vendor verification updated"
  }
}
```

---

### 21.4 Update Vendor Status
**PUT** `/api/admin/vendors/{id}/status`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "status": "SUSPENDED" // "ACTIVE" | "INACTIVE" | "SUSPENDED"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "vendor-1",
    "status": "SUSPENDED",
    "message": "Vendor status updated"
  }
}
```

---

## 22. Admin Listings Approval APIs

### 22.1 Get Pending Listings
**GET** `/api/admin/listings/pending?page=1&pageSize=10`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "listing-1",
      "name": "Golden Palms Convention Hall",
      "location": "JP Nagar, Bangalore",
      "vendorName": "Ramesh G",
      "vendorAvatar": "https://example.com/avatar.jpg",
      "type": "Hall",
      "submitted": "2024-11-10T08:30:00Z",
      "status": "PENDING" // "PENDING" | "NEEDS_CHANGES"
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 10,
    "total": 5,
    "totalPages": 1
  }
}
```

---

### 22.2 Get Listing Details
**GET** `/api/admin/listings/{id}`

**Headers:** `Authorization: Bearer {token}`

**Response:** Same as 14.2 with additional admin fields

---

### 22.3 Approve Listing
**PUT** `/api/admin/listings/{id}/approve`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "listing-1",
    "status": "APPROVED",
    "message": "Listing approved successfully"
  }
}
```

---

### 22.4 Reject Listing
**PUT** `/api/admin/listings/{id}/reject`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "notes": "Images quality not acceptable"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "listing-1",
    "status": "REJECTED",
    "message": "Listing rejected"
  }
}
```

---

### 22.5 Request Changes
**PUT** `/api/admin/listings/{id}/needs-changes`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "notes": "Please update images and add more amenities"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "listing-1",
    "status": "NEEDS_CHANGES",
    "message": "Changes requested"
  }
}
```

---

## 23. Admin Bookings APIs

### 23.1 Get All Bookings
**GET** `/api/admin/bookings?page=1&pageSize=10&status=CONFIRMED&startDate=2024-01-01&endDate=2024-12-31`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "booking-1",
      "bookingId": "BK-2024-001",
      "customerName": "Aditi Sharma",
      "venueName": "Golden Palms Convention Hall",
      "date": "2024-12-25",
      "amount": 150000,
      "status": "CONFIRMED"
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 10,
    "total": 8432,
    "totalPages": 844
  }
}
```

---

### 23.2 Get Booking Details
**GET** `/api/admin/bookings/{id}`

**Headers:** `Authorization: Bearer {token}`

**Response:** Same as 6.2

---

### 23.3 Update Booking Status
**PUT** `/api/admin/bookings/{id}/status`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "status": "CONFIRMED" // "PENDING" | "CONFIRMED" | "CANCELLED"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "booking-1",
    "status": "CONFIRMED",
    "message": "Booking status updated"
  }
}
```

---

## 24. Admin Payouts APIs

### 24.1 Get All Payouts
**GET** `/api/admin/payouts?page=1&pageSize=10&status=PENDING`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "payout-1",
      "vendorId": "vendor-1",
      "vendorName": "Ramesh G",
      "amount": 50000,
      "requestDate": "2024-11-10",
      "status": "PENDING", // "PENDING" | "PROCESSED" | "FAILED"
      "processedDate": null
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 10,
    "total": 25,
    "totalPages": 3
  }
}
```

---

### 24.2 Get Pending Payouts
**GET** `/api/admin/payouts/pending?page=1&pageSize=10`

**Headers:** `Authorization: Bearer {token}`

**Response:** Same as 24.1 filtered by PENDING status

---

### 24.3 Process Payout
**POST** `/api/admin/payouts/{id}/process`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "payout-1",
    "status": "PROCESSED",
    "transactionId": "TXN123456",
    "message": "Payout processed successfully"
  }
}
```

---

### 24.4 Update Payout Status
**PUT** `/api/admin/payouts/{id}/status`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "status": "PROCESSED",
  "transactionId": "TXN123456",
  "notes": "Processed via bank transfer"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "payout-1",
    "status": "PROCESSED",
    "message": "Payout status updated"
  }
}
```

---

## 25. Admin Reports APIs

### 25.1 Get Revenue Reports
**GET** `/api/admin/reports/revenue?startDate=2024-01-01&endDate=2024-12-31&format=json`

**Headers:** `Authorization: Bearer {token}`

**Query Parameters:**
- `format` (string, optional): json | csv | excel

**Response:**
```json
{
  "success": true,
  "data": {
    "period": {
      "startDate": "2024-01-01",
      "endDate": "2024-12-31"
    },
    "totalRevenue": 42500000,
    "totalCommission": 2125000,
    "breakdown": [
      {
        "month": "2024-01",
        "revenue": 3500000,
        "commission": 175000
      }
    ]
  }
}
```

---

### 25.2 Get Booking Reports
**GET** `/api/admin/reports/bookings?startDate=2024-01-01&endDate=2024-12-31&format=json`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "period": {
      "startDate": "2024-01-01",
      "endDate": "2024-12-31"
    },
    "totalBookings": 8432,
    "confirmed": 8000,
    "cancelled": 432,
    "breakdown": [
      {
        "month": "2024-01",
        "total": 650,
        "confirmed": 600,
        "cancelled": 50
      }
    ]
  }
}
```

---

### 25.3 Get Vendor Reports
**GET** `/api/admin/reports/vendors?startDate=2024-01-01&endDate=2024-12-31&format=json`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "period": {
      "startDate": "2024-01-01",
      "endDate": "2024-12-31"
    },
    "totalVendors": 1240,
    "activeVendors": 1000,
    "newVendors": 240,
    "breakdown": [
      {
        "month": "2024-01",
        "new": 20,
        "active": 950
      }
    ]
  }
}
```

---

### 25.4 Get User Reports
**GET** `/api/admin/reports/users?startDate=2024-01-01&endDate=2024-12-31&format=json`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "period": {
      "startDate": "2024-01-01",
      "endDate": "2024-12-31"
    },
    "totalUsers": 24582,
    "newUsers": 5000,
    "activeUsers": 15000,
    "breakdown": [
      {
        "month": "2024-01",
        "new": 250,
        "active": 1200
      }
    ]
  }
}
```

---

### 25.5 Export Reports
**GET** `/api/admin/reports/export?type=revenue&startDate=2024-01-01&endDate=2024-12-31&format=csv`

**Headers:** `Authorization: Bearer {token}`

**Query Parameters:**
- `type` (string): revenue | bookings | vendors | users
- `format` (string): csv | excel

**Response:** File download (CSV/Excel)

---

## 26. Admin Reviews APIs

### 26.1 Get All Reviews
**GET** `/api/admin/reviews?page=1&pageSize=10&status=published`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "review-1",
      "author": "John Doe",
      "listingId": "hall-1",
      "listingName": "Royal Grand Hall",
      "rating": 5,
      "comment": "Excellent venue!",
      "date": "2024-10-15",
      "status": "published", // "published" | "pending" | "rejected"
      "verified": true
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 10,
    "total": 500,
    "totalPages": 50
  }
}
```

---

### 26.2 Publish/Unpublish Review
**PUT** `/api/admin/reviews/{id}/publish`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "published": true
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "review-1",
    "status": "published",
    "message": "Review status updated"
  }
}
```

---

### 26.3 Delete Review
**DELETE** `/api/admin/reviews/{id}`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "message": "Review deleted successfully"
}
```

---

## 27. Admin Platform Settings APIs

### 27.1 Get All Settings
**GET** `/api/admin/settings`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "commissionRate": 5,
    "platformFee": 2,
    "minBookingAmount": 10000,
    "maxBookingDays": 365,
    "cancellationPolicy": "Standard policy",
    "supportEmail": "support@sambhramabook.com",
    "supportPhone": "+91 98765 43210"
  }
}
```

---

### 27.2 Get Specific Setting
**GET** `/api/admin/settings/{key}`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "data": {
    "key": "commissionRate",
    "value": 5,
    "description": "Platform commission rate in percentage"
  }
}
```

---

### 27.3 Update Setting
**PUT** `/api/admin/settings/{key}`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "value": 6
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "key": "commissionRate",
    "value": 6,
    "message": "Setting updated successfully"
  }
}
```

---

### 27.4 Create Setting
**POST** `/api/admin/settings`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "key": "newSetting",
  "value": "setting_value",
  "description": "Description of the setting"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "key": "newSetting",
    "value": "setting_value",
    "message": "Setting created successfully"
  }
}
```

---

## 28. Review APIs

### 28.1 Create Review
**POST** `/api/reviews`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "listingId": "hall-1",
  "rating": 5,
  "comment": "Excellent venue!",
  "verified": true
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "review-1",
    "message": "Review submitted successfully"
  }
}
```

---

### 28.2 Get Reviews for Listing
**GET** `/api/reviews/listing/{listingId}?page=1&pageSize=10`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "review-1",
      "author": "John Doe",
      "rating": 5,
      "comment": "Excellent venue!",
      "date": "2024-10-15",
      "verified": true
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 10,
    "total": 25,
    "totalPages": 3
  }
}
```

---

### 28.3 Mark Review as Helpful
**PUT** `/api/reviews/{id}/helpful`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "helpful": true
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "review-1",
    "helpfulCount": 10,
    "message": "Review marked as helpful"
  }
}
```

---

### 28.4 Add Vendor Response
**PUT** `/api/reviews/{id}/vendor-response`

**Headers:** `Authorization: Bearer {token}`

**Request:**
```json
{
  "response": "Thank you for your feedback!"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "review-1",
    "vendorResponse": "Thank you for your feedback!",
    "message": "Response added successfully"
  }
}
```

---

## 29. File Upload APIs

### 29.1 Upload Image
**POST** `/api/upload/image`

**Headers:** `Authorization: Bearer {token}`

**Request:** `multipart/form-data`
- `image` (file): Image file

**Response:**
```json
{
  "success": true,
  "data": {
    "fileId": "file-123",
    "url": "https://example.com/uploaded-image.jpg",
    "thumbnailUrl": "https://example.com/uploaded-image-thumb.jpg"
  }
}
```

---

### 29.2 Upload Multiple Images
**POST** `/api/upload/images`

**Headers:** `Authorization: Bearer {token}`

**Request:** `multipart/form-data`
- `images` (file[]): Array of image files

**Response:**
```json
{
  "success": true,
  "data": {
    "files": [
      {
        "fileId": "file-123",
        "url": "https://example.com/uploaded-image1.jpg"
      },
      {
        "fileId": "file-124",
        "url": "https://example.com/uploaded-image2.jpg"
      }
    ]
  }
}
```

---

### 29.3 Delete Uploaded File
**DELETE** `/api/upload/{fileId}`

**Headers:** `Authorization: Bearer {token}`

**Response:**
```json
{
  "success": true,
  "message": "File deleted successfully"
}
```

---

## 30. Search APIs

### 30.1 Global Search
**GET** `/api/search?q=wedding&type=hall&page=1&pageSize=10`

**Query Parameters:**
- `q` (string, required): Search query
- `type` (string, optional): hall | service | vendor | all
- `page` (number, default: 1)
- `pageSize` (number, default: 10)

**Response:**
```json
{
  "success": true,
  "data": {
    "halls": [
      {
        "id": "hall-1",
        "name": "Royal Grand Hall",
        "location": "Bangalore",
        "type": "hall"
      }
    ],
    "services": [
      {
        "id": "service-1",
        "name": "Wedding Photography",
        "location": "Bangalore",
        "type": "service"
      }
    ],
    "vendors": []
  },
  "pagination": {
    "page": 1,
    "pageSize": 10,
    "total": 25,
    "totalPages": 3
  }
}
```

---

## Error Response Format

All endpoints return errors in this format:

```json
{
  "success": false,
  "error": {
    "code": "ERROR_CODE",
    "message": "Human readable error message",
    "details": {
      "field": "Additional error details"
    }
  }
}
```

### Common Error Codes:
- `VALIDATION_ERROR` - Request validation failed
- `UNAUTHORIZED` - Authentication required
- `FORBIDDEN` - Insufficient permissions
- `NOT_FOUND` - Resource not found
- `CONFLICT` - Resource conflict (e.g., duplicate)
- `INTERNAL_ERROR` - Server error

---

## Authentication

Most endpoints require authentication via Bearer token in the Authorization header:

```
Authorization: Bearer {jwt_token}
```

---

## Pagination

Paginated responses include pagination metadata:

```json
{
  "pagination": {
    "page": 1,
    "pageSize": 10,
    "total": 100,
    "totalPages": 10
  }
}
```

---

## Date Formats

All dates are in ISO 8601 format: `YYYY-MM-DD` or `YYYY-MM-DDTHH:mm:ssZ`

---

## Notes

1. All monetary values are in INR (Indian Rupees)
2. All phone numbers should be in Indian format (10 digits)
3. Image URLs should be absolute URLs
4. File uploads support: JPG, PNG, WebP (max 10MB per file)
5. OTP expires in 5 minutes (300 seconds)
6. Booking reference IDs follow format: `SB-{YYYY}-{XXXXX}`
