# SambhramaBook Database Design
## Senior System Architect - Database Schema

### Overview
This document outlines the complete database schema for SambhramaBook, a platform for booking convention halls and event services. The design follows normalization principles, ensures data integrity, and supports scalability.

---

## Database: `sambhramabook_db`

---

## 1. USER MANAGEMENT

### 1.1 `users` Table
Stores all user accounts (Customers, Vendors, Admins)

```sql
CREATE TABLE users (
    id BIGSERIAL PRIMARY KEY,
    mobile VARCHAR(15) UNIQUE NOT NULL,
    email VARCHAR(255) UNIQUE,
    name VARCHAR(255) NOT NULL,
    role VARCHAR(20) NOT NULL CHECK (role IN ('User', 'Vendor', 'Admin')),
    password_hash VARCHAR(255), -- For future password-based auth
    is_active BOOLEAN DEFAULT TRUE,
    is_email_verified BOOLEAN DEFAULT FALSE,
    is_mobile_verified BOOLEAN DEFAULT FALSE,
    last_login_at TIMESTAMP,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP NULL,
    
    INDEX idx_users_mobile (mobile),
    INDEX idx_users_email (email),
    INDEX idx_users_role (role),
    INDEX idx_users_active (is_active)
);
```

### 1.2 `user_profiles` Table
Extended profile information for regular users

```sql
CREATE TABLE user_profiles (
    id BIGSERIAL PRIMARY KEY,
    user_id BIGINT UNIQUE NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    full_name VARCHAR(255),
    date_of_birth DATE,
    gender VARCHAR(20),
    profile_image_url TEXT,
    address_line1 VARCHAR(255),
    address_line2 VARCHAR(255),
    city VARCHAR(100),
    state VARCHAR(100),
    pincode VARCHAR(10),
    country VARCHAR(100) DEFAULT 'India',
    alternate_phone VARCHAR(15),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    INDEX idx_user_profiles_user_id (user_id)
);
```

### 1.3 `vendor_profiles` Table
Business information for vendors

```sql
CREATE TABLE vendor_profiles (
    id BIGSERIAL PRIMARY KEY,
    user_id BIGINT UNIQUE NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    business_name VARCHAR(255) NOT NULL,
    business_type VARCHAR(50) NOT NULL CHECK (business_type IN ('Hall Owner', 'Service Provider')),
    business_email VARCHAR(255) NOT NULL,
    business_phone VARCHAR(15) NOT NULL,
    business_logo_url TEXT,
    business_description TEXT,
    
    -- Address
    address_line1 VARCHAR(255) NOT NULL,
    address_line2 VARCHAR(255),
    city VARCHAR(100) NOT NULL,
    state VARCHAR(100) NOT NULL,
    pincode VARCHAR(10) NOT NULL,
    country VARCHAR(100) DEFAULT 'India',
    latitude DECIMAL(10, 8),
    longitude DECIMAL(11, 8),
    
    -- Business Details
    gst_number VARCHAR(15),
    pan_number VARCHAR(10),
    bank_account_number VARCHAR(50),
    ifsc_code VARCHAR(11),
    bank_name VARCHAR(255),
    account_holder_name VARCHAR(255),
    
    -- Status
    profile_complete BOOLEAN DEFAULT FALSE,
    is_verified BOOLEAN DEFAULT FALSE,
    verification_status VARCHAR(20) DEFAULT 'PENDING' CHECK (verification_status IN ('PENDING', 'APPROVED', 'REJECTED', 'SUSPENDED')),
    verification_notes TEXT,
    verified_at TIMESTAMP,
    verified_by BIGINT REFERENCES users(id),
    
    -- Statistics
    total_listings INT DEFAULT 0,
    total_bookings INT DEFAULT 0,
    total_earnings DECIMAL(12, 2) DEFAULT 0,
    average_rating DECIMAL(3, 2) DEFAULT 0,
    total_reviews INT DEFAULT 0,
    
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    INDEX idx_vendor_profiles_user_id (user_id),
    INDEX idx_vendor_profiles_city (city),
    INDEX idx_vendor_profiles_state (state),
    INDEX idx_vendor_profiles_type (business_type),
    INDEX idx_vendor_profiles_verified (is_verified),
    INDEX idx_vendor_profiles_location (latitude, longitude)
);
```

---

## 2. AUTHENTICATION & SECURITY

### 2.1 `otp_verifications` Table
Stores OTP codes for phone/email verification

```sql
CREATE TABLE otp_verifications (
    id BIGSERIAL PRIMARY KEY,
    mobile_or_email VARCHAR(255) NOT NULL,
    otp_code VARCHAR(6) NOT NULL,
    otp_type VARCHAR(20) NOT NULL CHECK (otp_type IN ('LOGIN', 'REGISTER', 'RESET_PASSWORD', 'VERIFY_EMAIL', 'VERIFY_MOBILE')),
    is_used BOOLEAN DEFAULT FALSE,
    expires_at TIMESTAMP NOT NULL,
    verified_at TIMESTAMP,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    INDEX idx_otp_mobile_email (mobile_or_email),
    INDEX idx_otp_code (otp_code),
    INDEX idx_otp_expires (expires_at)
);
```

### 2.2 `sessions` Table
User session management

```sql
CREATE TABLE sessions (
    id BIGSERIAL PRIMARY KEY,
    user_id BIGINT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    token VARCHAR(255) UNIQUE NOT NULL,
    refresh_token VARCHAR(255) UNIQUE,
    device_info TEXT,
    ip_address VARCHAR(45),
    user_agent TEXT,
    expires_at TIMESTAMP NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_activity_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    INDEX idx_sessions_user_id (user_id),
    INDEX idx_sessions_token (token),
    INDEX idx_sessions_expires (expires_at)
);
```

---

## 3. LISTINGS (HALLS & SERVICES)

### 3.1 `listings` Table
Main table for all listings (halls and services)

```sql
CREATE TABLE listings (
    id BIGSERIAL PRIMARY KEY,
    vendor_id BIGINT NOT NULL REFERENCES vendor_profiles(id) ON DELETE CASCADE,
    listing_type VARCHAR(20) NOT NULL CHECK (listing_type IN ('Hall', 'Service')),
    title VARCHAR(255) NOT NULL,
    slug VARCHAR(255) UNIQUE NOT NULL,
    description TEXT,
    short_description VARCHAR(500),
    
    -- Location
    address_line1 VARCHAR(255) NOT NULL,
    address_line2 VARCHAR(255),
    city VARCHAR(100) NOT NULL,
    state VARCHAR(100) NOT NULL,
    pincode VARCHAR(10) NOT NULL,
    country VARCHAR(100) DEFAULT 'India',
    latitude DECIMAL(10, 8),
    longitude DECIMAL(11, 8),
    
    -- For Halls
    capacity_min INT,
    capacity_max INT,
    area_sqft INT,
    parking_capacity INT,
    
    -- Pricing
    base_price DECIMAL(10, 2) NOT NULL,
    price_per_hour DECIMAL(10, 2),
    price_per_day DECIMAL(10, 2),
    currency VARCHAR(3) DEFAULT 'INR',
    cancellation_policy TEXT,
    
    -- Status
    status VARCHAR(20) DEFAULT 'DRAFT' CHECK (status IN ('DRAFT', 'PENDING_APPROVAL', 'APPROVED', 'REJECTED', 'SUSPENDED', 'INACTIVE')),
    approval_status VARCHAR(20) DEFAULT 'PENDING' CHECK (approval_status IN ('PENDING', 'APPROVED', 'NEEDS_CHANGES', 'REJECTED')),
    approval_notes TEXT,
    approved_at TIMESTAMP,
    approved_by BIGINT REFERENCES users(id),
    
    -- Statistics
    view_count INT DEFAULT 0,
    booking_count INT DEFAULT 0,
    average_rating DECIMAL(3, 2) DEFAULT 0,
    total_reviews INT DEFAULT 0,
    
    -- SEO
    meta_title VARCHAR(255),
    meta_description TEXT,
    meta_keywords TEXT,
    
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    published_at TIMESTAMP,
    deleted_at TIMESTAMP NULL,
    
    INDEX idx_listings_vendor_id (vendor_id),
    INDEX idx_listings_type (listing_type),
    INDEX idx_listings_status (status),
    INDEX idx_listings_approval (approval_status),
    INDEX idx_listings_city (city),
    INDEX idx_listings_state (state),
    INDEX idx_listings_location (latitude, longitude),
    INDEX idx_listings_slug (slug),
    FULLTEXT INDEX idx_listings_search (title, description, short_description)
);
```

### 3.2 `listing_images` Table
Images for listings

```sql
CREATE TABLE listing_images (
    id BIGSERIAL PRIMARY KEY,
    listing_id BIGINT NOT NULL REFERENCES listings(id) ON DELETE CASCADE,
    image_url TEXT NOT NULL,
    image_type VARCHAR(20) DEFAULT 'GALLERY' CHECK (image_type IN ('THUMBNAIL', 'GALLERY', 'FLOOR_PLAN', 'EXTERIOR', 'INTERIOR')),
    display_order INT DEFAULT 0,
    alt_text VARCHAR(255),
    is_primary BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    INDEX idx_listing_images_listing_id (listing_id),
    INDEX idx_listing_images_order (listing_id, display_order)
);
```

### 3.3 `listing_amenities` Table
Amenities/facilities for listings

```sql
CREATE TABLE listing_amenities (
    id BIGSERIAL PRIMARY KEY,
    listing_id BIGINT NOT NULL REFERENCES listings(id) ON DELETE CASCADE,
    amenity_name VARCHAR(100) NOT NULL,
    amenity_category VARCHAR(50), -- e.g., 'BASIC', 'PREMIUM', 'FOOD', 'TECHNOLOGY'
    icon_url TEXT,
    is_available BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    INDEX idx_listing_amenities_listing_id (listing_id),
    INDEX idx_listing_amenities_category (amenity_category)
);
```

### 3.4 `service_packages` Table
Service packages for service-type listings

```sql
CREATE TABLE service_packages (
    id BIGSERIAL PRIMARY KEY,
    listing_id BIGINT NOT NULL REFERENCES listings(id) ON DELETE CASCADE,
    package_name VARCHAR(255) NOT NULL,
    description TEXT,
    price DECIMAL(10, 2) NOT NULL,
    duration_hours INT,
    includes TEXT, -- JSON array of included items
    display_order INT DEFAULT 0,
    is_popular BOOLEAN DEFAULT FALSE,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    INDEX idx_service_packages_listing_id (listing_id),
    INDEX idx_service_packages_active (is_active)
);
```

### 3.5 `listing_availability` Table
Availability calendar for listings

```sql
CREATE TABLE listing_availability (
    id BIGSERIAL PRIMARY KEY,
    listing_id BIGINT NOT NULL REFERENCES listings(id) ON DELETE CASCADE,
    date DATE NOT NULL,
    status VARCHAR(20) DEFAULT 'AVAILABLE' CHECK (status IN ('AVAILABLE', 'BOOKED', 'BLOCKED', 'MAINTENANCE')),
    price_override DECIMAL(10, 2), -- Special pricing for specific dates
    notes TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    UNIQUE KEY unique_listing_date (listing_id, date),
    INDEX idx_listing_availability_listing_id (listing_id),
    INDEX idx_listing_availability_date (date),
    INDEX idx_listing_availability_status (status)
);
```

---

## 4. BOOKINGS

### 4.1 `bookings` Table
Main bookings table

```sql
CREATE TABLE bookings (
    id BIGSERIAL PRIMARY KEY,
    booking_reference VARCHAR(50) UNIQUE NOT NULL, -- e.g., SB-8824-X901
    customer_id BIGINT NOT NULL REFERENCES users(id) ON DELETE RESTRICT,
    vendor_id BIGINT NOT NULL REFERENCES vendor_profiles(id) ON DELETE RESTRICT,
    listing_id BIGINT NOT NULL REFERENCES listings(id) ON DELETE RESTRICT,
    
    -- Booking Details
    event_type VARCHAR(100), -- e.g., 'Wedding Reception', 'Corporate Event'
    event_name VARCHAR(255),
    guest_count INT NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    start_time TIME,
    end_time TIME,
    duration_days INT NOT NULL,
    
    -- Service Package (if service booking)
    service_package_id BIGINT REFERENCES service_packages(id),
    
    -- Pricing
    base_amount DECIMAL(10, 2) NOT NULL,
    discount_amount DECIMAL(10, 2) DEFAULT 0,
    tax_amount DECIMAL(10, 2) DEFAULT 0,
    platform_fee DECIMAL(10, 2) DEFAULT 0,
    total_amount DECIMAL(10, 2) NOT NULL,
    amount_paid DECIMAL(10, 2) DEFAULT 0,
    refund_amount DECIMAL(10, 2) DEFAULT 0,
    
    -- Status
    status VARCHAR(20) DEFAULT 'PENDING' CHECK (status IN ('PENDING', 'CONFIRMED', 'PAID', 'CANCELLED', 'COMPLETED', 'REFUNDED')),
    payment_status VARCHAR(20) DEFAULT 'PENDING' CHECK (payment_status IN ('PENDING', 'PAID', 'FAILED', 'REFUNDED', 'PARTIAL_REFUND')),
    
    -- Payment
    payment_method VARCHAR(50), -- 'UPI', 'CARD', 'NETBANKING', 'WALLET'
    payment_transaction_id VARCHAR(255),
    payment_date TIMESTAMP,
    
    -- Cancellation
    cancellation_reason TEXT,
    cancellation_fee DECIMAL(10, 2) DEFAULT 0,
    cancelled_at TIMESTAMP,
    cancelled_by BIGINT REFERENCES users(id),
    
    -- Special Requirements
    special_requirements TEXT,
    additional_notes TEXT,
    
    -- Vendor Response
    vendor_status VARCHAR(20) DEFAULT 'PENDING' CHECK (vendor_status IN ('PENDING', 'APPROVED', 'REJECTED')),
    vendor_response_notes TEXT,
    vendor_responded_at TIMESTAMP,
    
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    confirmed_at TIMESTAMP,
    completed_at TIMESTAMP,
    
    INDEX idx_bookings_customer_id (customer_id),
    INDEX idx_bookings_vendor_id (vendor_id),
    INDEX idx_bookings_listing_id (listing_id),
    INDEX idx_bookings_reference (booking_reference),
    INDEX idx_bookings_status (status),
    INDEX idx_bookings_payment_status (payment_status),
    INDEX idx_bookings_dates (start_date, end_date),
    INDEX idx_bookings_created (created_at)
);
```

### 4.2 `booking_guests` Table
Guest information for bookings

```sql
CREATE TABLE booking_guests (
    id BIGSERIAL PRIMARY KEY,
    booking_id BIGINT NOT NULL REFERENCES bookings(id) ON DELETE CASCADE,
    guest_name VARCHAR(255) NOT NULL,
    guest_email VARCHAR(255),
    guest_phone VARCHAR(15),
    is_primary_contact BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    INDEX idx_booking_guests_booking_id (booking_id)
);
```

### 4.3 `booking_timeline` Table
Track booking status changes

```sql
CREATE TABLE booking_timeline (
    id BIGSERIAL PRIMARY KEY,
    booking_id BIGINT NOT NULL REFERENCES bookings(id) ON DELETE CASCADE,
    status_from VARCHAR(20),
    status_to VARCHAR(20) NOT NULL,
    changed_by BIGINT REFERENCES users(id),
    notes TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    INDEX idx_booking_timeline_booking_id (booking_id),
    INDEX idx_booking_timeline_created (created_at)
);
```

---

## 5. PAYMENTS & TRANSACTIONS

### 5.1 `payments` Table
Payment transactions

```sql
CREATE TABLE payments (
    id BIGSERIAL PRIMARY KEY,
    booking_id BIGINT NOT NULL REFERENCES bookings(id) ON DELETE RESTRICT,
    payment_reference VARCHAR(100) UNIQUE NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    currency VARCHAR(3) DEFAULT 'INR',
    payment_method VARCHAR(50) NOT NULL,
    payment_gateway VARCHAR(50), -- 'RAZORPAY', 'STRIPE', 'PAYU', etc.
    gateway_transaction_id VARCHAR(255),
    gateway_response TEXT, -- JSON response from gateway
    
    status VARCHAR(20) DEFAULT 'PENDING' CHECK (status IN ('PENDING', 'SUCCESS', 'FAILED', 'REFUNDED', 'PARTIAL_REFUND')),
    failure_reason TEXT,
    
    paid_at TIMESTAMP,
    refunded_at TIMESTAMP,
    refund_amount DECIMAL(10, 2) DEFAULT 0,
    refund_reason TEXT,
    
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    INDEX idx_payments_booking_id (booking_id),
    INDEX idx_payments_reference (payment_reference),
    INDEX idx_payments_status (status),
    INDEX idx_payments_gateway_id (gateway_transaction_id)
);
```

### 5.2 `payouts` Table
Vendor payouts

```sql
CREATE TABLE payouts (
    id BIGSERIAL PRIMARY KEY,
    vendor_id BIGINT NOT NULL REFERENCES vendor_profiles(id) ON DELETE RESTRICT,
    booking_id BIGINT REFERENCES bookings(id),
    amount DECIMAL(10, 2) NOT NULL,
    platform_commission DECIMAL(10, 2) NOT NULL,
    net_amount DECIMAL(10, 2) NOT NULL,
    
    status VARCHAR(20) DEFAULT 'PENDING' CHECK (status IN ('PENDING', 'PROCESSING', 'COMPLETED', 'FAILED', 'CANCELLED')),
    payout_method VARCHAR(50), -- 'BANK_TRANSFER', 'UPI', 'CHEQUE'
    transaction_reference VARCHAR(255),
    bank_account_id BIGINT, -- Reference to vendor's bank account
    
    processed_at TIMESTAMP,
    processed_by BIGINT REFERENCES users(id),
    failure_reason TEXT,
    
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    INDEX idx_payouts_vendor_id (vendor_id),
    INDEX idx_payouts_booking_id (booking_id),
    INDEX idx_payouts_status (status),
    INDEX idx_payouts_created (created_at)
);
```

---

## 6. REVIEWS & RATINGS

### 6.1 `reviews` Table
Customer reviews for listings

```sql
CREATE TABLE reviews (
    id BIGSERIAL PRIMARY KEY,
    booking_id BIGINT UNIQUE NOT NULL REFERENCES bookings(id) ON DELETE CASCADE,
    listing_id BIGINT NOT NULL REFERENCES listings(id) ON DELETE CASCADE,
    customer_id BIGINT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    vendor_id BIGINT NOT NULL REFERENCES vendor_profiles(id) ON DELETE CASCADE,
    
    rating INT NOT NULL CHECK (rating >= 1 AND rating <= 5),
    title VARCHAR(255),
    comment TEXT,
    
    -- Detailed Ratings
    cleanliness_rating INT CHECK (cleanliness_rating >= 1 AND cleanliness_rating <= 5),
    service_rating INT CHECK (service_rating >= 1 AND service_rating <= 5),
    value_rating INT CHECK (value_rating >= 1 AND value_rating <= 5),
    location_rating INT CHECK (location_rating >= 1 AND location_rating <= 5),
    
    is_verified_booking BOOLEAN DEFAULT TRUE,
    is_published BOOLEAN DEFAULT TRUE,
    is_helpful_count INT DEFAULT 0,
    
    vendor_response TEXT,
    vendor_responded_at TIMESTAMP,
    
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    INDEX idx_reviews_booking_id (booking_id),
    INDEX idx_reviews_listing_id (listing_id),
    INDEX idx_reviews_customer_id (customer_id),
    INDEX idx_reviews_vendor_id (vendor_id),
    INDEX idx_reviews_rating (rating),
    INDEX idx_reviews_published (is_published)
);
```

### 6.2 `review_helpful` Table
Track helpful votes on reviews

```sql
CREATE TABLE review_helpful (
    id BIGSERIAL PRIMARY KEY,
    review_id BIGINT NOT NULL REFERENCES reviews(id) ON DELETE CASCADE,
    user_id BIGINT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    is_helpful BOOLEAN NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    UNIQUE KEY unique_review_user (review_id, user_id),
    INDEX idx_review_helpful_review_id (review_id)
);
```

---

## 7. NOTIFICATIONS

### 7.1 `notifications` Table
System notifications

```sql
CREATE TABLE notifications (
    id BIGSERIAL PRIMARY KEY,
    user_id BIGINT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    notification_type VARCHAR(50) NOT NULL, -- 'BOOKING_CONFIRMED', 'PAYMENT_RECEIVED', 'REVIEW_POSTED', etc.
    title VARCHAR(255) NOT NULL,
    message TEXT NOT NULL,
    data TEXT, -- JSON data for additional context
    
    is_read BOOLEAN DEFAULT FALSE,
    read_at TIMESTAMP,
    
    action_url VARCHAR(500), -- Deep link to relevant page
    priority VARCHAR(20) DEFAULT 'NORMAL' CHECK (priority IN ('LOW', 'NORMAL', 'HIGH', 'URGENT')),
    
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    INDEX idx_notifications_user_id (user_id),
    INDEX idx_notifications_read (is_read),
    INDEX idx_notifications_created (created_at),
    INDEX idx_notifications_type (notification_type)
);
```

---

## 8. SAVED ITEMS

### 8.1 `saved_listings` Table
User's saved/favorite listings

```sql
CREATE TABLE saved_listings (
    id BIGSERIAL PRIMARY KEY,
    user_id BIGINT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    listing_id BIGINT NOT NULL REFERENCES listings(id) ON DELETE CASCADE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    UNIQUE KEY unique_user_listing (user_id, listing_id),
    INDEX idx_saved_listings_user_id (user_id),
    INDEX idx_saved_listings_listing_id (listing_id)
);
```

---

## 9. PLATFORM SETTINGS

### 9.1 `platform_settings` Table
Global platform configuration

```sql
CREATE TABLE platform_settings (
    id BIGSERIAL PRIMARY KEY,
    setting_key VARCHAR(100) UNIQUE NOT NULL,
    setting_value TEXT NOT NULL,
    setting_type VARCHAR(50) DEFAULT 'STRING' CHECK (setting_type IN ('STRING', 'NUMBER', 'BOOLEAN', 'JSON')),
    description TEXT,
    category VARCHAR(50), -- 'PAYMENT', 'COMMISSION', 'NOTIFICATION', 'GENERAL'
    is_public BOOLEAN DEFAULT FALSE, -- Can be accessed via public API
    updated_by BIGINT REFERENCES users(id),
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    INDEX idx_platform_settings_key (setting_key),
    INDEX idx_platform_settings_category (category)
);
```

**Default Settings:**
- `platform_name`: "SambhramaBook"
- `commission_rate`: "10" (percentage)
- `min_booking_amount`: "1000"
- `max_booking_amount`: "10000000"
- `support_email`: "support@sambhramabook.com"
- `support_phone`: "+91-800-123-4567"
- `enable_notifications`: "true"
- `enable_email_alerts`: "true"

---

## 10. ANALYTICS & REPORTING

### 10.1 `analytics_events` Table
Track user actions and events

```sql
CREATE TABLE analytics_events (
    id BIGSERIAL PRIMARY KEY,
    user_id BIGINT REFERENCES users(id) ON DELETE SET NULL,
    event_type VARCHAR(100) NOT NULL, -- 'PAGE_VIEW', 'LISTING_VIEW', 'BOOKING_CREATED', etc.
    event_category VARCHAR(50),
    entity_type VARCHAR(50), -- 'LISTING', 'BOOKING', 'USER', etc.
    entity_id BIGINT,
    properties TEXT, -- JSON object with event properties
    session_id VARCHAR(255),
    ip_address VARCHAR(45),
    user_agent TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    INDEX idx_analytics_events_user_id (user_id),
    INDEX idx_analytics_events_type (event_type),
    INDEX idx_analytics_events_category (event_category),
    INDEX idx_analytics_events_created (created_at),
    INDEX idx_analytics_events_entity (entity_type, entity_id)
);
```

### 10.2 `daily_statistics` Table
Aggregated daily statistics

```sql
CREATE TABLE daily_statistics (
    id BIGSERIAL PRIMARY KEY,
    stat_date DATE NOT NULL UNIQUE,
    
    -- User Stats
    total_users INT DEFAULT 0,
    new_users INT DEFAULT 0,
    active_users INT DEFAULT 0,
    
    -- Vendor Stats
    total_vendors INT DEFAULT 0,
    new_vendors INT DEFAULT 0,
    active_vendors INT DEFAULT 0,
    
    -- Listing Stats
    total_listings INT DEFAULT 0,
    new_listings INT DEFAULT 0,
    approved_listings INT DEFAULT 0,
    
    -- Booking Stats
    total_bookings INT DEFAULT 0,
    new_bookings INT DEFAULT 0,
    confirmed_bookings INT DEFAULT 0,
    cancelled_bookings INT DEFAULT 0,
    booking_revenue DECIMAL(12, 2) DEFAULT 0,
    
    -- Platform Stats
    platform_revenue DECIMAL(12, 2) DEFAULT 0,
    vendor_payouts DECIMAL(12, 2) DEFAULT 0,
    
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    INDEX idx_daily_statistics_date (stat_date)
);
```

---

## 11. RELATIONSHIPS SUMMARY

```
users (1) ──< (1) user_profiles
users (1) ──< (1) vendor_profiles
users (1) ──< (*) sessions
users (1) ──< (*) bookings (as customer)
users (1) ──< (*) reviews
users (1) ──< (*) notifications
users (1) ──< (*) saved_listings

vendor_profiles (1) ──< (*) listings
vendor_profiles (1) ──< (*) bookings (as vendor)
vendor_profiles (1) ──< (*) payouts

listings (1) ──< (*) listing_images
listings (1) ──< (*) listing_amenities
listings (1) ──< (*) service_packages
listings (1) ──< (*) listing_availability
listings (1) ──< (*) bookings
listings (1) ──< (*) reviews
listings (1) ──< (*) saved_listings

bookings (1) ──< (1) payments
bookings (1) ──< (*) booking_guests
bookings (1) ──< (*) booking_timeline
bookings (1) ──< (1) reviews

service_packages (1) ──< (*) bookings
```

---

## 12. INDEXES & PERFORMANCE

### Composite Indexes for Common Queries:
```sql
-- Fast listing search
CREATE INDEX idx_listings_search_composite ON listings(city, state, listing_type, status, approval_status);

-- Fast booking queries
CREATE INDEX idx_bookings_vendor_status ON bookings(vendor_id, status, created_at DESC);
CREATE INDEX idx_bookings_customer_status ON bookings(customer_id, status, created_at DESC);
CREATE INDEX idx_bookings_date_range ON bookings(listing_id, start_date, end_date, status);

-- Fast availability checks
CREATE INDEX idx_availability_listing_date_status ON listing_availability(listing_id, date, status);
```

---

## 13. TRIGGERS & STORED PROCEDURES

### 13.1 Update Timestamps Trigger
```sql
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ language 'plpgsql';

-- Apply to all tables with updated_at
CREATE TRIGGER update_users_updated_at BEFORE UPDATE ON users
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
-- Repeat for other tables...
```

### 13.2 Generate Booking Reference
```sql
CREATE OR REPLACE FUNCTION generate_booking_reference()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.booking_reference IS NULL OR NEW.booking_reference = '' THEN
        NEW.booking_reference := 'SB-' || 
            TO_CHAR(NOW(), 'YYMMDD') || '-' || 
            UPPER(SUBSTRING(MD5(RANDOM()::TEXT) FROM 1 FOR 4));
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER set_booking_reference BEFORE INSERT ON bookings
    FOR EACH ROW EXECUTE FUNCTION generate_booking_reference();
```

### 13.3 Update Listing Statistics
```sql
CREATE OR REPLACE FUNCTION update_listing_stats()
RETURNS TRIGGER AS $$
BEGIN
    -- Update booking count
    UPDATE listings 
    SET booking_count = (
        SELECT COUNT(*) FROM bookings 
        WHERE listing_id = NEW.listing_id AND status IN ('CONFIRMED', 'PAID', 'COMPLETED')
    )
    WHERE id = NEW.listing_id;
    
    -- Update average rating
    UPDATE listings 
    SET average_rating = (
        SELECT AVG(rating) FROM reviews 
        WHERE listing_id = NEW.listing_id AND is_published = TRUE
    ),
    total_reviews = (
        SELECT COUNT(*) FROM reviews 
        WHERE listing_id = NEW.listing_id AND is_published = TRUE
    )
    WHERE id = NEW.listing_id;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_listing_stats_on_review AFTER INSERT OR UPDATE ON reviews
    FOR EACH ROW EXECUTE FUNCTION update_listing_stats();
```

---

## 14. DATA RETENTION & ARCHIVING

### 14.1 Soft Deletes
- All major tables use `deleted_at` for soft deletes
- Archive old bookings after 2 years
- Archive old analytics events after 1 year

### 14.2 Partitioning (Future)
- Consider partitioning `bookings` by `created_at` (monthly)
- Consider partitioning `analytics_events` by `created_at` (monthly)
- Consider partitioning `daily_statistics` by `stat_date` (yearly)

---

## 15. SECURITY CONSIDERATIONS

1. **Row Level Security (RLS)** - Implement for multi-tenant data isolation
2. **Encryption** - Encrypt sensitive fields (bank accounts, PAN, etc.)
3. **Audit Logs** - Track all data modifications
4. **Backup Strategy** - Daily backups with 30-day retention
5. **Access Control** - Role-based access at database level

---

## 16. MIGRATION STRATEGY

1. Create database and schema
2. Insert default platform settings
3. Migrate existing data (if any)
4. Set up indexes
5. Configure triggers
6. Set up replication (for production)
7. Configure backups

---

## 17. ESTIMATED TABLE SIZES (Year 1)

- `users`: ~50,000 rows
- `vendor_profiles`: ~5,000 rows
- `listings`: ~10,000 rows
- `bookings`: ~100,000 rows
- `payments`: ~100,000 rows
- `reviews`: ~30,000 rows
- `notifications`: ~500,000 rows
- `analytics_events`: ~10,000,000 rows (consider archiving)

---

This database design provides:
✅ Scalability for growth
✅ Data integrity with constraints
✅ Performance with proper indexing
✅ Flexibility for future features
✅ Audit trail capabilities
✅ Multi-tenant support ready

