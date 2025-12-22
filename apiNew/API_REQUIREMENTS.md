# SambhramaBook API Requirements

## Complete API List Based on UI Pages and Components

### Authentication & Authorization APIs

#### 1. Authentication
- `POST /api/auth/send-otp` - Send OTP to mobile/email
- `POST /api/auth/verify-otp` - Verify OTP and login
- `POST /api/auth/logout` - Logout user
- `GET /api/auth/me` - Get current user info
- `POST /api/auth/refresh-token` - Refresh access token

#### 2. Vendor Onboarding
- `POST /api/vendor/onboarding` - Complete vendor onboarding
- `GET /api/vendor/onboarding/status` - Check onboarding status
- `PUT /api/vendor/onboarding` - Update onboarding data

---

### Landing Page APIs

#### 3. Popular Halls
- `GET /api/halls/popular?limit=3` - Get popular/featured halls

#### 4. Services Categories
- `GET /api/services/categories` - Get all service categories

---

### Halls Listing & Search APIs

#### 5. Hall Search & Filter
- `GET /api/halls?location=&minPrice=&maxPrice=&minCapacity=&maxCapacity=&amenities=&date=&days=&guests=&page=&pageSize=` - Search and filter halls

#### 6. Hall Details
- `GET /api/halls/{id}` - Get hall details by ID
- `GET /api/halls/slug/{slug}` - Get hall details by slug
- `GET /api/halls/{id}/availability?month=&year=` - Get availability calendar
- `GET /api/halls/{id}/reviews?page=&pageSize=` - Get reviews for a hall

---

### Services APIs

#### 7. Service Listing
- `GET /api/services/{type}` - Get services by type (photography, catering, etc.)
- `GET /api/services/{type}?location=&filters=...` - Filter services

#### 8. Service Details
- `GET /api/services/{type}/{id}` - Get service vendor details
- `GET /api/services/{type}/{id}/packages` - Get service packages
- `GET /api/services/{type}/{id}/portfolio` - Get portfolio images
- `GET /api/services/{type}/{id}/reviews` - Get reviews

---

### Booking APIs

#### 9. Create Booking
- `POST /api/bookings` - Create new booking
- `GET /api/bookings/{id}` - Get booking details
- `GET /api/bookings/reference/{reference}` - Get booking by reference

#### 10. Booking Management
- `PUT /api/bookings/{id}/cancel` - Cancel booking
- `PUT /api/bookings/{id}/reschedule` - Reschedule booking
- `POST /api/bookings/{id}/payment` - Process payment

#### 11. User Bookings
- `GET /api/user/bookings` - Get user's bookings
- `GET /api/user/bookings/{id}` - Get specific booking

#### 12. Vendor Bookings
- `GET /api/vendor/bookings` - Get vendor's bookings
- `PUT /api/vendor/bookings/{id}/approve` - Approve booking
- `PUT /api/vendor/bookings/{id}/reject` - Reject booking
- `POST /api/vendor/bookings` - Create booking manually (vendor)

---

### Payment APIs

#### 13. Payment Processing
- `POST /api/payments/initiate` - Initiate payment
- `POST /api/payments/verify` - Verify payment (webhook)
- `POST /api/payments/{id}/refund` - Process refund

---

### User Dashboard APIs

#### 14. User Profile
- `GET /api/user/profile` - Get user profile
- `PUT /api/user/profile` - Update user profile
- `PUT /api/user/profile/password` - Change password

#### 15. Saved Venues
- `GET /api/user/saved` - Get saved listings
- `POST /api/user/saved/{listingId}` - Save a listing
- `DELETE /api/user/saved/{listingId}` - Remove saved listing

#### 16. Notifications
- `GET /api/user/notifications` - Get notifications
- `PUT /api/user/notifications/{id}/read` - Mark as read
- `PUT /api/user/notifications/read-all` - Mark all as read
- `GET /api/user/notifications/unread-count` - Get unread count

---

### Vendor Dashboard APIs

#### 17. Vendor Overview
- `GET /api/vendor/dashboard/overview` - Get dashboard stats
- `GET /api/vendor/dashboard/recent-bookings` - Get recent bookings
- `GET /api/vendor/dashboard/listings` - Get vendor listings summary

#### 18. Vendor Listings
- `GET /api/vendor/listings` - Get all vendor listings
- `GET /api/vendor/listings/{id}` - Get listing details
- `POST /api/vendor/listings` - Create new listing
- `PUT /api/vendor/listings/{id}` - Update listing
- `DELETE /api/vendor/listings/{id}` - Delete listing
- `POST /api/vendor/listings/{id}/images` - Upload listing images
- `PUT /api/vendor/listings/{id}/status` - Update listing status (draft/publish)

#### 19. Vendor Bookings
- `GET /api/vendor/bookings` - Get all bookings
- `GET /api/vendor/bookings/pending` - Get pending bookings
- `PUT /api/vendor/bookings/{id}/approve` - Approve booking
- `PUT /api/vendor/bookings/{id}/reject` - Reject booking

#### 20. Vendor Availability
- `GET /api/vendor/availability/{listingId}?month=&year=` - Get availability
- `PUT /api/vendor/availability/{listingId}` - Update availability (bulk)
- `POST /api/vendor/availability/{listingId}/block` - Block dates
- `DELETE /api/vendor/availability/{listingId}/unblock` - Unblock dates

#### 21. Vendor Earnings
- `GET /api/vendor/earnings` - Get earnings summary
- `GET /api/vendor/earnings/transactions` - Get earnings transactions
- `GET /api/vendor/earnings/payouts` - Get payout history

#### 22. Vendor Settings
- `GET /api/vendor/settings` - Get vendor settings
- `PUT /api/vendor/settings` - Update vendor settings

---

### Admin Dashboard APIs

#### 23. Admin Overview
- `GET /api/admin/dashboard/overview` - Get admin dashboard stats
- `GET /api/admin/dashboard/pending-listings` - Get pending listings
- `GET /api/admin/dashboard/pending-payouts` - Get pending payouts

#### 24. Admin Analytics
- `GET /api/admin/analytics/summary` - Get analytics summary
- `GET /api/admin/analytics/revenue` - Get revenue analytics
- `GET /api/admin/analytics/users` - Get user analytics
- `GET /api/admin/analytics/bookings` - Get booking analytics

#### 25. Admin Users
- `GET /api/admin/users` - Get all users (paginated)
- `GET /api/admin/users/{id}` - Get user details
- `PUT /api/admin/users/{id}/status` - Update user status
- `DELETE /api/admin/users/{id}` - Delete user

#### 26. Admin Vendors
- `GET /api/admin/vendors` - Get all vendors (paginated)
- `GET /api/admin/vendors/{id}` - Get vendor details
- `PUT /api/admin/vendors/{id}/verify` - Verify/reject vendor
- `PUT /api/admin/vendors/{id}/status` - Update vendor status

#### 27. Admin Listings Approval
- `GET /api/admin/listings/pending` - Get pending listings
- `GET /api/admin/listings/{id}` - Get listing details
- `PUT /api/admin/listings/{id}/approve` - Approve listing
- `PUT /api/admin/listings/{id}/reject` - Reject listing (with notes)
- `PUT /api/admin/listings/{id}/needs-changes` - Request changes

#### 28. Admin Bookings
- `GET /api/admin/bookings` - Get all bookings (paginated, filtered)
- `GET /api/admin/bookings/{id}` - Get booking details
- `PUT /api/admin/bookings/{id}/status` - Update booking status

#### 29. Admin Payouts
- `GET /api/admin/payouts` - Get all payouts
- `GET /api/admin/payouts/pending` - Get pending payouts
- `POST /api/admin/payouts/{id}/process` - Process payout
- `PUT /api/admin/payouts/{id}/status` - Update payout status

#### 30. Admin Reports
- `GET /api/admin/reports/revenue` - Revenue reports
- `GET /api/admin/reports/bookings` - Booking reports
- `GET /api/admin/reports/vendors` - Vendor reports
- `GET /api/admin/reports/users` - User reports
- `GET /api/admin/reports/export` - Export reports (CSV/Excel)

#### 31. Admin Reviews
- `GET /api/admin/reviews` - Get all reviews
- `PUT /api/admin/reviews/{id}/publish` - Publish/unpublish review
- `DELETE /api/admin/reviews/{id}` - Delete review

#### 32. Admin Platform Settings
- `GET /api/admin/settings` - Get all platform settings
- `GET /api/admin/settings/{key}` - Get specific setting
- `PUT /api/admin/settings/{key}` - Update setting
- `POST /api/admin/settings` - Create new setting

---

### Review APIs

#### 33. Reviews
- `POST /api/reviews` - Create review
- `GET /api/reviews/listing/{listingId}` - Get reviews for listing
- `PUT /api/reviews/{id}/helpful` - Mark review as helpful
- `PUT /api/reviews/{id}/vendor-response` - Add vendor response

---

### Common/Utility APIs

#### 34. File Upload
- `POST /api/upload/image` - Upload image
- `POST /api/upload/images` - Upload multiple images
- `DELETE /api/upload/{fileId}` - Delete uploaded file

#### 35. Search
- `GET /api/search?q=&type=` - Global search (halls, services, vendors)

---

## API Response Standards

### Success Response
```json
{
  "success": true,
  "data": { ... },
  "message": "Operation successful"
}
```

### Error Response
```json
{
  "success": false,
  "error": {
    "code": "ERROR_CODE",
    "message": "Human readable error message",
    "details": { ... }
  }
}
```

### Paginated Response
```json
{
  "success": true,
  "data": [ ... ],
  "pagination": {
    "page": 1,
    "pageSize": 10,
    "total": 100,
    "totalPages": 10
  }
}
```

---

## Validation Rules

### User Registration/Login
- Mobile: Required, 10 digits, Indian format
- Email: Optional, valid email format
- OTP: Required, 6 digits

### Vendor Onboarding
- Business Name: Required, 2-255 characters
- Business Type: Required, must be 'Hall Owner' or 'Service Provider'
- Business Email: Required, valid email
- Business Phone: Required, 10 digits
- Address: Required
- City: Required, 2-100 characters
- State: Required, 2-100 characters
- Pincode: Required, 6 digits

### Listing Creation
- Title: Required, 3-255 characters
- Description: Optional, max 5000 characters
- Base Price: Required, > 0
- Capacity: Required for halls, min < max
- Location: Required (address, city, state, pincode)

### Booking Creation
- Listing ID: Required, must exist
- Start Date: Required, must be future date
- End Date: Required, >= start date
- Guest Count: Required, > 0
- Contact Info: Required (name, email or phone)

---

## External Tools/Paid Services Required

### 1. Payment Gateway
- **Razorpay** (Recommended for India) - Free tier available, 2% transaction fee
- **Stripe** - International, 2.9% + ₹2 per transaction
- **PayU** - Indian payment gateway

### 2. SMS Service (OTP)
- **Twilio** - $0.0075 per SMS
- **MSG91** (India) - ₹0.20-0.30 per SMS
- **TextLocal** (India) - ₹0.20 per SMS

### 3. Email Service
- **SendGrid** - Free tier: 100 emails/day
- **Mailgun** - Free tier: 5,000 emails/month
- **Amazon SES** - $0.10 per 1,000 emails

### 4. File Storage
- **AWS S3** - $0.023 per GB/month
- **Azure Blob Storage** - $0.0184 per GB/month
- **Cloudinary** - Free tier: 25 GB storage, 25 GB bandwidth

### 5. Maps & Geocoding
- **Google Maps API** - $5 per 1,000 requests
- **Mapbox** - Free tier: 50,000 requests/month

### 6. Analytics
- **Google Analytics** - Free
- **Application Insights** - Free tier: 5 GB/month

---

## Recommended Stack (Free/Open Source Alternatives)

1. **Payment**: Razorpay (Free tier for testing)
2. **SMS**: MSG91 or TextLocal (Low cost for India)
3. **Email**: SendGrid (Free tier sufficient for development)
4. **Storage**: Azure Blob Storage or AWS S3 (Pay as you go)
5. **Maps**: Mapbox (Free tier sufficient initially)

---

## Next Steps

1. ✅ Domain entities created
2. ✅ Infrastructure layer with EF Core setup
3. ⏳ Application layer with handlers
4. ⏳ API controllers
5. ⏳ Validation with FluentValidation
6. ⏳ Error handling middleware
7. ⏳ Authentication/Authorization
8. ⏳ Swagger documentation

