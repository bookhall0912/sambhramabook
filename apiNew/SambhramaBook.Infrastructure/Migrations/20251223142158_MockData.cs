using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SambhramaBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MockData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // =====================================================
            // 1. USERS (Vendors and Customers)
            // =====================================================

            // Vendor Users (for hall owners)
            migrationBuilder.Sql(@"
                INSERT INTO users (mobile, email, name, role, is_active, is_email_verified, is_mobile_verified, created_at, updated_at)
                VALUES
                    ('+91 98765 43210', 'vendor1@example.com', 'Ramesh G', 2, true, true, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    ('+91 98765 43211', 'vendor2@example.com', 'Suresh K', 2, true, true, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    ('+91 98765 43212', 'vendor3@example.com', 'Karthik R', 2, true, true, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    ('+91 98765 43213', 'vendor4@example.com', 'Anjali P', 2, true, true, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    ('+91 98765 43214', 'vendor5@example.com', 'Priya M', 2, true, true, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    ('+91 98765 43215', 'vendor6@example.com', 'Rajesh V', 2, true, true, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    ('+91 98765 43216', 'vendor7@example.com', 'Lakshmi N', 2, true, true, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    ('+91 98765 43217', 'vendor8@example.com', 'Mohan S', 2, true, true, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    ('+91 98765 43218', 'vendor9@example.com', 'Sunita K', 2, true, true, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    ('+91 98765 43219', 'vendor10@example.com', 'Vikram R', 2, true, true, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
            ");

            // Customer Users (for bookings and reviews)
            migrationBuilder.Sql(@"
                INSERT INTO users (mobile, email, name, role, is_active, is_email_verified, is_mobile_verified, created_at, updated_at)
                VALUES
                    ('+91 98765 44001', 'ananya.rao@example.com', 'Ananya Rao', 1, true, true, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    ('+91 98765 44002', 'rahul.verma@example.com', 'Rahul Verma', 1, true, true, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    ('+91 98765 44003', 'priya.sharma@example.com', 'Priya Sharma', 1, true, true, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    ('+91 98765 44004', 'amit.verma@example.com', 'Amit Verma', 1, true, true, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    ('+91 98765 44005', 'sneha.reddy@example.com', 'Sneha Reddy', 1, true, true, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    ('+91 98765 44006', 'karthik.m@example.com', 'Karthik M', 1, true, true, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
            ");

            // =====================================================
            // 2. VENDOR PROFILES
            // =====================================================

            migrationBuilder.Sql(@"
                INSERT INTO vendor_profiles (user_id, business_name, business_type, business_email, business_phone, address_line1, city, state, pincode, country, latitude, longitude, profile_complete, is_verified, verification_status, created_at, updated_at)
                VALUES
                    (1, 'Royal Palace Events', 1, 'vendor1@example.com', '+91 98765 43210', '4th Block, Jayanagar', 'Bangalore', 'Karnataka', '560011', 'India', 12.9716, 77.5946, true, true, 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    (2, 'Sree Krishna Events', 1, 'vendor2@example.com', '+91 98765 43211', 'Malleswaram', 'Bangalore', 'Karnataka', '560003', 'India', 12.9981, 77.5708, true, true, 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    (3, 'Golden Pearl Events', 1, 'vendor3@example.com', '+91 98765 43212', 'Mysore Road', 'Bangalore', 'Karnataka', '560026', 'India', 12.9352, 77.5345, true, true, 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    (4, 'Elegant Events Management', 1, 'vendor4@example.com', '+91 98765 43213', 'Whitefield', 'Bangalore', 'Karnataka', '560066', 'India', 12.9698, 77.7499, true, true, 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    (5, 'Grand Celebration Events', 1, 'vendor5@example.com', '+91 98765 43214', 'Indiranagar', 'Bangalore', 'Karnataka', '560038', 'India', 12.9784, 77.6408, true, true, 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    (6, 'Platinum Events', 1, 'vendor6@example.com', '+91 98765 43215', 'Koramangala', 'Bangalore', 'Karnataka', '560095', 'India', 12.9352, 77.6245, true, true, 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    (7, 'Grand Lotus Events', 1, 'vendor7@example.com', '+91 98765 43216', 'Koramangala', 'Bangalore', 'Karnataka', '560095', 'India', 12.9352, 77.6245, true, true, 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    (8, 'Anugraha Events', 1, 'vendor8@example.com', '+91 98765 43217', 'Malleswaram', 'Bangalore', 'Karnataka', '560003', 'India', 12.9981, 77.5708, true, true, 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    (9, 'Silver Oak Events', 1, 'vendor9@example.com', '+91 98765 43218', 'Indiranagar', 'Bangalore', 'Karnataka', '560038', 'India', 12.9784, 77.6408, true, true, 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    (10, 'Imperial Gardens Events', 1, 'vendor10@example.com', '+91 98765 43219', 'Whitefield', 'Bangalore', 'Karnataka', '560066', 'India', 12.9698, 77.7499, true, true, 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
            ");

            // =====================================================
            // 3. LISTINGS (Halls)
            // =====================================================

            migrationBuilder.Sql(@"
                INSERT INTO listings (vendor_id, listing_type, title, slug, description, short_description, address_line1, city, state, pincode, country, latitude, longitude, capacity_min, capacity_max, base_price, currency, cancellation_policy, status, approval_status, average_rating, total_reviews, booking_count, view_count, approved_at, published_at, created_at, updated_at)
                VALUES
                    (1, 1, 'Royal Palace Convention Center', 'royal-palace-convention-center', 'Royal Palace Convention Center stands as one of Bangalore''s most prestigious wedding venues, blending traditional elegance with modern luxury for grand celebrations. Our venue offers state-of-the-art facilities, professional event management services, and a team dedicated to making your special day unforgettable.', 'Royal Palace Convention Center stands as one of Bangalore''s most prestigious wedding venues.', '4th Block, Jayanagar', 'Bangalore', 'Karnataka', '560011', 'India', 12.9716, 77.5946, 800, 1200, 150000.00, 'INR', 'Full refund if cancelled 30 days before event', 3, 2, 4.90, 128, 45, 1250, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    (2, 1, 'Sree Krishna Convention Hall', 'sree-krishna-convention-hall', 'A traditional venue perfect for intimate celebrations.', 'A traditional venue perfect for intimate celebrations.', 'Malleswaram', 'Bangalore', 'Karnataka', '560003', 'India', 12.9981, 77.5708, 300, 500, 75000.00, 'INR', NULL, 3, 2, 4.60, 95, 32, 890, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    (3, 1, 'Golden Pearl Grand', 'golden-pearl-grand', 'Elegant venue with modern amenities and spacious halls.', 'Elegant venue with modern amenities and spacious halls.', 'Mysore Road', 'Bangalore', 'Karnataka', '560026', 'India', 12.9352, 77.5345, 500, 800, 95000.00, 'INR', NULL, 3, 2, 4.70, 112, 38, 1020, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    (4, 1, 'Elegant Events Venue', 'elegant-events-venue', 'Premium venue with world-class facilities and excellent service.', 'Premium venue with world-class facilities and excellent service.', 'Whitefield', 'Bangalore', 'Karnataka', '560066', 'India', 12.9698, 77.7499, 600, 1000, 135000.00, 'INR', NULL, 3, 2, 4.80, 145, 52, 1450, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    (5, 1, 'Grand Celebration Hall', 'grand-celebration-hall', 'Spacious hall with modern amenities and professional catering services.', 'Spacious hall with modern amenities and professional catering services.', 'Indiranagar', 'Bangalore', 'Karnataka', '560038', 'India', 12.9784, 77.6408, 500, 800, 120000.00, 'INR', NULL, 3, 2, 4.70, 98, 41, 980, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    (6, 1, 'Platinum Convention Center', 'platinum-convention-center', 'Luxury venue with state-of-the-art facilities and premium services.', 'Luxury venue with state-of-the-art facilities and premium services.', 'Koramangala', 'Bangalore', 'Karnataka', '560095', 'India', 12.9352, 77.6245, 1000, 1500, 180000.00, 'INR', NULL, 3, 2, 4.90, 167, 68, 1890, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    (7, 1, 'Grand Lotus Venue', 'grand-lotus-venue', 'Premium wedding venue with elegant interiors and excellent service.', 'Premium wedding venue with elegant interiors and excellent service.', 'Koramangala', 'Bangalore', 'Karnataka', '560095', 'India', 12.9352, 77.6245, 800, 1000, 225000.00, 'INR', NULL, 3, 2, 4.80, 95, 28, 850, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    (8, 1, 'Anugraha Hall', 'anugraha-hall', 'Traditional hall perfect for weddings and celebrations.', 'Traditional hall perfect for weddings and celebrations.', 'Malleswaram', 'Bangalore', 'Karnataka', '560003', 'India', 12.9981, 77.5708, 400, 600, 60000.00, 'INR', NULL, 3, 2, 4.60, 78, 22, 720, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    (9, 1, 'Silver Oak Banquets', 'silver-oak-banquets', 'Elegant banquet hall with modern facilities.', 'Elegant banquet hall with modern facilities.', 'Indiranagar', 'Bangalore', 'Karnataka', '560038', 'India', 12.9784, 77.6408, 500, 700, 85000.00, 'INR', NULL, 3, 2, 4.70, 102, 35, 920, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    (10, 1, 'Imperial Gardens', 'imperial-gardens', 'Grand venue with beautiful gardens and premium facilities.', 'Grand venue with beautiful gardens and premium facilities.', 'Whitefield', 'Bangalore', 'Karnataka', '560066', 'India', 12.9698, 77.7499, 1500, 2000, 300000.00, 'INR', NULL, 3, 2, 4.90, 156, 58, 1650, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
            ");

            // =====================================================
            // 4. LISTING IMAGES
            // =====================================================

            migrationBuilder.Sql(@"
                INSERT INTO listing_images (listing_id, image_url, image_type, display_order, is_primary, alt_text, created_at)
                VALUES
                    (1, 'https://images.unsplash.com/photo-1528605248644-14dd04022da1?w=800&h=600&fit=crop', 'GALLERY', 0, true, 'Royal Palace Convention Center - Main Hall', CURRENT_TIMESTAMP),
                    (1, 'https://images.unsplash.com/photo-1522335789203-aabd1fc54bc9?w=800&h=600&fit=crop', 'GALLERY', 1, false, 'Royal Palace Convention Center - Interior', CURRENT_TIMESTAMP),
                    (1, 'https://images.unsplash.com/photo-1519741497674-611481863552?w=800&h=600&fit=crop', 'GALLERY', 2, false, 'Royal Palace Convention Center - Stage', CURRENT_TIMESTAMP),
                    (2, 'https://images.unsplash.com/photo-1519741497674-611481863552?w=800&h=600&fit=crop', 'GALLERY', 0, true, 'Sree Krishna Convention Hall', CURRENT_TIMESTAMP),
                    (3, 'https://images.unsplash.com/photo-1522335789203-aabd1fc54bc9?w=800&h=600&fit=crop', 'GALLERY', 0, true, 'Golden Pearl Grand', CURRENT_TIMESTAMP),
                    (4, 'https://images.unsplash.com/photo-1464366400600-7168b8af9bc3?w=800&h=600&fit=crop', 'GALLERY', 0, true, 'Elegant Events Venue', CURRENT_TIMESTAMP),
                    (5, 'https://images.unsplash.com/photo-1511578314322-379afb476865?w=800&h=600&fit=crop', 'GALLERY', 0, true, 'Grand Celebration Hall', CURRENT_TIMESTAMP),
                    (6, 'https://images.unsplash.com/photo-1519167758481-83f550bb49b3?w=800&h=600&fit=crop', 'GALLERY', 0, true, 'Platinum Convention Center', CURRENT_TIMESTAMP),
                    (7, 'https://images.unsplash.com/photo-1519741497674-611481863552?w=800&h=600&fit=crop', 'GALLERY', 0, true, 'Grand Lotus Venue', CURRENT_TIMESTAMP),
                    (8, 'https://images.unsplash.com/photo-1522335789203-aabd1fc54bc9?w=800&h=600&fit=crop', 'GALLERY', 0, true, 'Anugraha Hall', CURRENT_TIMESTAMP),
                    (9, 'https://images.unsplash.com/photo-1528605248644-14dd04022da1?w=800&h=600&fit=crop', 'GALLERY', 0, true, 'Silver Oak Banquets', CURRENT_TIMESTAMP),
                    (10, 'https://images.unsplash.com/photo-1519741497674-611481863552?w=800&h=600&fit=crop', 'GALLERY', 0, true, 'Imperial Gardens', CURRENT_TIMESTAMP);
            ");

            // =====================================================
            // 5. LISTING AMENITIES
            // =====================================================

            migrationBuilder.Sql(@"
                INSERT INTO listing_amenities (listing_id, amenity_name, amenity_category, is_available, created_at)
                VALUES
                    (1, 'AC', 'BASIC', true, CURRENT_TIMESTAMP),
                    (1, 'Centralized Air Conditioning', 'BASIC', true, CURRENT_TIMESTAMP),
                    (1, 'In-house Catering', 'FOOD', true, CURRENT_TIMESTAMP),
                    (1, 'Sound System', 'TECHNOLOGY', true, CURRENT_TIMESTAMP),
                    (1, 'Live Streaming', 'TECHNOLOGY', true, CURRENT_TIMESTAMP),
                    (1, '24/7 Power Backup', 'BASIC', true, CURRENT_TIMESTAMP),
                    (1, 'Bridal Makeup Room', 'PREMIUM', true, CURRENT_TIMESTAMP),
                    (2, 'Basic Facilities', 'BASIC', true, CURRENT_TIMESTAMP),
                    (3, 'AC', 'BASIC', true, CURRENT_TIMESTAMP),
                    (4, 'AC', 'BASIC', true, CURRENT_TIMESTAMP),
                    (4, 'Valet Parking', 'PREMIUM', true, CURRENT_TIMESTAMP),
                    (5, 'AC', 'BASIC', true, CURRENT_TIMESTAMP),
                    (5, 'Guest Rooms', 'PREMIUM', true, CURRENT_TIMESTAMP),
                    (6, 'AC', 'BASIC', true, CURRENT_TIMESTAMP),
                    (6, 'Valet Parking', 'PREMIUM', true, CURRENT_TIMESTAMP),
                    (6, 'Guest Rooms', 'PREMIUM', true, CURRENT_TIMESTAMP),
                    (7, 'AC', 'BASIC', true, CURRENT_TIMESTAMP),
                    (7, 'Valet Parking', 'PREMIUM', true, CURRENT_TIMESTAMP),
                    (8, 'AC', 'BASIC', true, CURRENT_TIMESTAMP),
                    (9, 'AC', 'BASIC', true, CURRENT_TIMESTAMP),
                    (9, 'Valet Parking', 'PREMIUM', true, CURRENT_TIMESTAMP),
                    (10, 'AC', 'BASIC', true, CURRENT_TIMESTAMP),
                    (10, 'Valet Parking', 'PREMIUM', true, CURRENT_TIMESTAMP),
                    (10, 'Guest Rooms', 'PREMIUM', true, CURRENT_TIMESTAMP);
            ");

            // =====================================================
            // 6. BOOKINGS (From Vendor Dashboard Mock Data)
            // =====================================================
            // Note: Customer IDs are 11-16 (inserted after vendors 1-10)
            // Vendor IDs are 1-10

            migrationBuilder.Sql(@"
                INSERT INTO bookings (booking_reference, customer_id, vendor_id, listing_id, event_type, guest_count, start_date, end_date, duration_days, base_amount, total_amount, status, payment_status, payment_method, payment_transaction_id, payment_date, vendor_status, confirmed_at, created_at, updated_at)
                VALUES
                    ('SB-9012', (SELECT id FROM users WHERE mobile = '+91 98765 44004'), 1, 1, 'Wedding Reception', 500, '2024-11-12', '2024-11-13', 2, 300000.00, 300000.00, 2, 2, 'UPI', 'TXN9012', '2024-10-15 10:30:00', 'APPROVED', '2024-10-15 10:30:00', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    ('SB-9013', (SELECT id FROM users WHERE mobile = '+91 98765 44005'), 1, 1, 'Wedding Reception', 300, '2024-11-15', '2024-11-15', 1, 150000.00, 150000.00, 1, 1, NULL, NULL, NULL, 'PENDING', NULL, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    ('SB-9014', (SELECT id FROM users WHERE mobile = '+91 98765 44006'), 1, 1, 'Wedding Reception', 400, '2024-11-22', '2024-11-22', 1, 200000.00, 200000.00, 2, 2, 'UPI', 'TXN9014', '2024-10-20 14:20:00', 'APPROVED', '2024-10-20 14:20:00', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    ('SB-9015', (SELECT id FROM users WHERE mobile = '+91 98765 44005'), 1, 1, 'Wedding Reception', 600, '2024-12-01', '2024-12-03', 3, 450000.00, 450000.00, 1, 1, NULL, NULL, NULL, 'PENDING', NULL, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    ('SB-9001', (SELECT id FROM users WHERE mobile = '+91 98765 44002'), 1, 1, 'Wedding Reception', 450, '2024-11-20', '2024-11-20', 1, 180000.00, 180000.00, 5, 2, 'UPI', 'TXN9001', '2024-10-10 09:00:00', 'APPROVED', '2024-10-10 09:00:00', '2024-10-01 10:00:00', CURRENT_TIMESTAMP),
                    ('SB-9002', (SELECT id FROM users WHERE mobile = '+91 98765 44003'), 1, 1, 'Wedding Reception', 600, '2024-10-10', '2024-10-10', 1, 200000.00, 200000.00, 5, 2, 'UPI', 'TXN9002', '2024-09-15 11:00:00', 'APPROVED', '2024-09-15 11:00:00', '2024-09-01 10:00:00', CURRENT_TIMESTAMP);
            ");

            // Update completed_at for completed bookings
            migrationBuilder.Sql(@"
                UPDATE bookings SET completed_at = '2024-11-21 00:00:00' WHERE booking_reference = 'SB-9001';
                UPDATE bookings SET completed_at = '2024-10-11 00:00:00' WHERE booking_reference = 'SB-9002';
            ");

            // =====================================================
            // 7. REVIEWS (For Hall 1 - Royal Palace Convention Center)
            // =====================================================
            // Using booking_reference to find booking_id

            migrationBuilder.Sql(@"
                INSERT INTO reviews (booking_id, listing_id, customer_id, vendor_id, rating, comment, is_verified_booking, is_published, created_at, updated_at)
                SELECT 
                    b.id,
                    1,
                    (SELECT id FROM users WHERE mobile = '+91 98765 44001'),
                    1,
                    5,
                    'Stunning venue with excellent management. Everything was perfect!',
                    true,
                    true,
                    '2024-12-15 10:00:00',
                    CURRENT_TIMESTAMP
                FROM bookings b WHERE b.booking_reference = 'SB-9012';
            ");

            migrationBuilder.Sql(@"
                INSERT INTO reviews (booking_id, listing_id, customer_id, vendor_id, rating, comment, is_verified_booking, is_published, created_at, updated_at)
                SELECT 
                    b.id,
                    1,
                    (SELECT id FROM users WHERE mobile = '+91 98765 44002'),
                    1,
                    5,
                    'Great location and facilities. Highly recommended for large events.',
                    true,
                    true,
                    '2024-11-20 18:00:00',
                    CURRENT_TIMESTAMP
                FROM bookings b WHERE b.booking_reference = 'SB-9001';
            ");

            migrationBuilder.Sql(@"
                INSERT INTO reviews (booking_id, listing_id, customer_id, vendor_id, rating, comment, is_verified_booking, is_published, created_at, updated_at)
                SELECT 
                    b.id,
                    1,
                    (SELECT id FROM users WHERE mobile = '+91 98765 44003'),
                    1,
                    5,
                    'Beautiful venue with professional staff. Made our wedding day perfect!',
                    true,
                    true,
                    '2024-10-10 20:00:00',
                    CURRENT_TIMESTAMP
                FROM bookings b WHERE b.booking_reference = 'SB-9002';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete in reverse order of dependencies

            // Delete Reviews (by booking reference)
            migrationBuilder.Sql(@"
                DELETE FROM reviews WHERE booking_id IN (
                    SELECT id FROM bookings WHERE booking_reference IN ('SB-9012', 'SB-9001', 'SB-9002')
                );
            ");

            // Delete Bookings
            migrationBuilder.Sql(@"
                DELETE FROM bookings WHERE booking_reference IN ('SB-9012', 'SB-9013', 'SB-9014', 'SB-9015', 'SB-9001', 'SB-9002');
            ");

            // Delete Listing Amenities (by listing slug)
            migrationBuilder.Sql(@"
                DELETE FROM listing_amenities WHERE listing_id IN (
                    SELECT id FROM listings WHERE slug IN (
                        'royal-palace-convention-center', 'sree-krishna-convention-hall', 'golden-pearl-grand',
                        'elegant-events-venue', 'grand-celebration-hall', 'platinum-convention-center',
                        'grand-lotus-venue', 'anugraha-hall', 'silver-oak-banquets', 'imperial-gardens'
                    )
                );
            ");

            // Delete Listing Images (by listing slug)
            migrationBuilder.Sql(@"
                DELETE FROM listing_images WHERE listing_id IN (
                    SELECT id FROM listings WHERE slug IN (
                        'royal-palace-convention-center', 'sree-krishna-convention-hall', 'golden-pearl-grand',
                        'elegant-events-venue', 'grand-celebration-hall', 'platinum-convention-center',
                        'grand-lotus-venue', 'anugraha-hall', 'silver-oak-banquets', 'imperial-gardens'
                    )
                );
            ");

            // Delete Listings (by slug)
            migrationBuilder.Sql(@"
                DELETE FROM listings WHERE slug IN (
                    'royal-palace-convention-center', 'sree-krishna-convention-hall', 'golden-pearl-grand',
                    'elegant-events-venue', 'grand-celebration-hall', 'platinum-convention-center',
                    'grand-lotus-venue', 'anugraha-hall', 'silver-oak-banquets', 'imperial-gardens'
                );
            ");

            // Delete Vendor Profiles (by business name)
            migrationBuilder.Sql(@"
                DELETE FROM vendor_profiles WHERE business_name IN (
                    'Royal Palace Events', 'Sree Krishna Events', 'Golden Pearl Events',
                    'Elegant Events Management', 'Grand Celebration Events', 'Platinum Events',
                    'Grand Lotus Events', 'Anugraha Events', 'Silver Oak Events', 'Imperial Gardens Events'
                );
            ");

            // Delete Users (customers first, then vendors)
            migrationBuilder.Sql(@"
                DELETE FROM users WHERE mobile IN (
                    '+91 98765 44001', '+91 98765 44002', '+91 98765 44003',
                    '+91 98765 44004', '+91 98765 44005', '+91 98765 44006'
                );
            ");

            migrationBuilder.Sql(@"
                DELETE FROM users WHERE mobile IN (
                    '+91 98765 43210', '+91 98765 43211', '+91 98765 43212',
                    '+91 98765 43213', '+91 98765 43214', '+91 98765 43215',
                    '+91 98765 43216', '+91 98765 43217', '+91 98765 43218',
                    '+91 98765 43219'
                );
            ");
        }
    }
}
