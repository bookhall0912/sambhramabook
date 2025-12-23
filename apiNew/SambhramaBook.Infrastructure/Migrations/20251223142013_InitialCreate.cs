using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SambhramaBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "BookingReferenceSeq",
                startValue: 1000L,
                minValue: 1000L,
                maxValue: 999999L,
                cyclic: true);

            migrationBuilder.CreateTable(
                name: "otp_verifications",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mobile_or_email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    otp_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    otp_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_used = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    verified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_otp_verifications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "platform_settings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    setting_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    setting_value = table.Column<string>(type: "text", nullable: false),
                    setting_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "STRING"),
                    description = table.Column<string>(type: "text", nullable: true),
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    is_public = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    updated_by = table.Column<long>(type: "bigint", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platform_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mobile = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    is_email_verified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_mobile_verified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    last_login_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    notification_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    data = table.Column<string>(type: "text", nullable: true),
                    is_read = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    read_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    action_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    priority = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "NORMAL"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_notifications_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    refresh_token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    device_info = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ip_address = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    user_agent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    last_activity_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessions", x => x.id);
                    table.ForeignKey(
                        name: "FK_sessions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_profiles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    full_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    profile_image_url = table.Column<string>(type: "text", nullable: true),
                    address_line1 = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    address_line2 = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    state = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    pincode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "India"),
                    alternate_phone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_profiles", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_profiles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vendor_profiles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    business_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    business_type = table.Column<int>(type: "integer", nullable: false),
                    business_email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    business_phone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    business_logo_url = table.Column<string>(type: "text", nullable: true),
                    business_description = table.Column<string>(type: "text", nullable: true),
                    address_line1 = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    address_line2 = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    state = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    pincode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "India"),
                    latitude = table.Column<decimal>(type: "numeric(10,8)", precision: 10, scale: 8, nullable: true),
                    longitude = table.Column<decimal>(type: "numeric(11,8)", precision: 11, scale: 8, nullable: true),
                    gst_number = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    pan_number = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    bank_account_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ifsc_code = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true),
                    bank_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    account_holder_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    profile_complete = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_verified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    verification_status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    verification_notes = table.Column<string>(type: "text", nullable: true),
                    verified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    verified_by = table.Column<long>(type: "bigint", nullable: true),
                    total_listings = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    total_bookings = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    total_earnings = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false, defaultValue: 0m),
                    average_rating = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: false, defaultValue: 0m),
                    total_reviews = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vendor_profiles", x => x.id);
                    table.ForeignKey(
                        name: "FK_vendor_profiles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "listings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vendor_id = table.Column<long>(type: "bigint", nullable: false),
                    listing_type = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    short_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    address_line1 = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    address_line2 = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    state = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    pincode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "India"),
                    latitude = table.Column<decimal>(type: "numeric(10,8)", precision: 10, scale: 8, nullable: true),
                    longitude = table.Column<decimal>(type: "numeric(11,8)", precision: 11, scale: 8, nullable: true),
                    capacity_min = table.Column<int>(type: "integer", nullable: true),
                    capacity_max = table.Column<int>(type: "integer", nullable: true),
                    area_sqft = table.Column<int>(type: "integer", nullable: true),
                    parking_capacity = table.Column<int>(type: "integer", nullable: true),
                    base_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    price_per_hour = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    price_per_day = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false, defaultValue: "INR"),
                    cancellation_policy = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    approval_status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    approval_notes = table.Column<string>(type: "text", nullable: true),
                    approved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    approved_by = table.Column<long>(type: "bigint", nullable: true),
                    view_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    booking_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    average_rating = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: false, defaultValue: 0m),
                    total_reviews = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    meta_title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    meta_description = table.Column<string>(type: "text", nullable: true),
                    meta_keywords = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    published_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listings", x => x.id);
                    table.ForeignKey(
                        name: "FK_listings_vendor_profiles_vendor_id",
                        column: x => x.vendor_id,
                        principalTable: "vendor_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "listing_amenities",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    listing_id = table.Column<long>(type: "bigint", nullable: false),
                    amenity_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    amenity_category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    icon_url = table.Column<string>(type: "text", nullable: true),
                    is_available = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listing_amenities", x => x.id);
                    table.ForeignKey(
                        name: "FK_listing_amenities_listings_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "listing_availability",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    listing_id = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    price_override = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listing_availability", x => x.id);
                    table.ForeignKey(
                        name: "FK_listing_availability_listings_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "listing_images",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    listing_id = table.Column<long>(type: "bigint", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: false),
                    image_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "GALLERY"),
                    display_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    alt_text = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listing_images", x => x.id);
                    table.ForeignKey(
                        name: "FK_listing_images_listings_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "saved_listings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    listing_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saved_listings", x => x.id);
                    table.ForeignKey(
                        name: "FK_saved_listings_listings_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_saved_listings_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_packages",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    listing_id = table.Column<long>(type: "bigint", nullable: false),
                    package_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    duration_hours = table.Column<int>(type: "integer", nullable: true),
                    includes = table.Column<string>(type: "text", nullable: true),
                    display_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_popular = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_packages", x => x.id);
                    table.ForeignKey(
                        name: "FK_service_packages_listings_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    booking_reference = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    customer_id = table.Column<long>(type: "bigint", nullable: false),
                    vendor_id = table.Column<long>(type: "bigint", nullable: false),
                    listing_id = table.Column<long>(type: "bigint", nullable: false),
                    event_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    event_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    guest_count = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    end_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    duration_days = table.Column<int>(type: "integer", nullable: false),
                    service_package_id = table.Column<long>(type: "bigint", nullable: true),
                    base_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    discount_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    tax_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    platform_fee = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    total_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    amount_paid = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    refund_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    payment_status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    payment_method = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    payment_transaction_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    payment_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cancellation_reason = table.Column<string>(type: "text", nullable: true),
                    cancellation_fee = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    cancelled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cancelled_by = table.Column<long>(type: "bigint", nullable: true),
                    special_requirements = table.Column<string>(type: "text", nullable: true),
                    additional_notes = table.Column<string>(type: "text", nullable: true),
                    vendor_status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "PENDING"),
                    vendor_response_notes = table.Column<string>(type: "text", nullable: true),
                    vendor_responded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    confirmed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.id);
                    table.ForeignKey(
                        name: "FK_bookings_listings_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bookings_service_packages_service_package_id",
                        column: x => x.service_package_id,
                        principalTable: "service_packages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_bookings_users_customer_id",
                        column: x => x.customer_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bookings_vendor_profiles_vendor_id",
                        column: x => x.vendor_id,
                        principalTable: "vendor_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "booking_guests",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    booking_id = table.Column<long>(type: "bigint", nullable: false),
                    guest_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    guest_email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    guest_phone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    is_primary_contact = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_guests", x => x.id);
                    table.ForeignKey(
                        name: "FK_booking_guests_bookings_booking_id",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "booking_timelines",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    booking_id = table.Column<long>(type: "bigint", nullable: false),
                    status_from = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    status_to = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    changed_by = table.Column<long>(type: "bigint", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_timelines", x => x.id);
                    table.ForeignKey(
                        name: "FK_booking_timelines_bookings_booking_id",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    booking_id = table.Column<long>(type: "bigint", nullable: false),
                    payment_reference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false, defaultValue: "INR"),
                    payment_method = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    payment_gateway = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    gateway_transaction_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    gateway_response = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    failure_reason = table.Column<string>(type: "text", nullable: true),
                    paid_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    refunded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    refund_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    refund_reason = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_payments_bookings_booking_id",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "payouts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vendor_id = table.Column<long>(type: "bigint", nullable: false),
                    booking_id = table.Column<long>(type: "bigint", nullable: true),
                    amount = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    platform_commission = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    net_amount = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "PENDING"),
                    payout_method = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    transaction_reference = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    bank_account_id = table.Column<long>(type: "bigint", nullable: true),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    processed_by = table.Column<long>(type: "bigint", nullable: true),
                    failure_reason = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    VendorProfileId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payouts", x => x.id);
                    table.ForeignKey(
                        name: "FK_payouts_bookings_booking_id",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_payouts_vendor_profiles_VendorProfileId",
                        column: x => x.VendorProfileId,
                        principalTable: "vendor_profiles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_payouts_vendor_profiles_vendor_id",
                        column: x => x.vendor_id,
                        principalTable: "vendor_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    booking_id = table.Column<long>(type: "bigint", nullable: false),
                    listing_id = table.Column<long>(type: "bigint", nullable: false),
                    customer_id = table.Column<long>(type: "bigint", nullable: false),
                    vendor_id = table.Column<long>(type: "bigint", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    comment = table.Column<string>(type: "text", nullable: true),
                    cleanliness_rating = table.Column<int>(type: "integer", nullable: true),
                    service_rating = table.Column<int>(type: "integer", nullable: true),
                    value_rating = table.Column<int>(type: "integer", nullable: true),
                    location_rating = table.Column<int>(type: "integer", nullable: true),
                    is_verified_booking = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    is_published = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    is_helpful_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    vendor_response = table.Column<string>(type: "text", nullable: true),
                    vendor_responded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    VendorProfileId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.id);
                    table.ForeignKey(
                        name: "FK_reviews_bookings_booking_id",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reviews_listings_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reviews_users_customer_id",
                        column: x => x.customer_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reviews_vendor_profiles_VendorProfileId",
                        column: x => x.VendorProfileId,
                        principalTable: "vendor_profiles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_reviews_vendor_profiles_vendor_id",
                        column: x => x.vendor_id,
                        principalTable: "vendor_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "review_helpfuls",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    review_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    is_helpful = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review_helpfuls", x => x.id);
                    table.ForeignKey(
                        name: "FK_review_helpfuls_reviews_review_id",
                        column: x => x.review_id,
                        principalTable: "reviews",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_review_helpfuls_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_booking_guests_booking_id",
                table: "booking_guests",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_timelines_booking_id",
                table: "booking_timelines",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_timelines_created_at",
                table: "booking_timelines",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_booking_reference",
                table: "bookings",
                column: "booking_reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookings_created_at",
                table: "bookings",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_customer_id",
                table: "bookings",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_listing_id",
                table: "bookings",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_payment_status",
                table: "bookings",
                column: "payment_status");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_service_package_id",
                table: "bookings",
                column: "service_package_id");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_start_date_end_date",
                table: "bookings",
                columns: new[] { "start_date", "end_date" });

            migrationBuilder.CreateIndex(
                name: "IX_bookings_status",
                table: "bookings",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_vendor_id",
                table: "bookings",
                column: "vendor_id");

            migrationBuilder.CreateIndex(
                name: "IX_listing_amenities_amenity_category",
                table: "listing_amenities",
                column: "amenity_category");

            migrationBuilder.CreateIndex(
                name: "IX_listing_amenities_listing_id",
                table: "listing_amenities",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "IX_listing_availability_date",
                table: "listing_availability",
                column: "date");

            migrationBuilder.CreateIndex(
                name: "IX_listing_availability_listing_id",
                table: "listing_availability",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "IX_listing_availability_listing_id_date",
                table: "listing_availability",
                columns: new[] { "listing_id", "date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_listing_availability_status",
                table: "listing_availability",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_listing_images_listing_id",
                table: "listing_images",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "IX_listing_images_listing_id_is_primary",
                table: "listing_images",
                columns: new[] { "listing_id", "is_primary" });

            migrationBuilder.CreateIndex(
                name: "IX_listings_approval_status",
                table: "listings",
                column: "approval_status");

            migrationBuilder.CreateIndex(
                name: "IX_listings_city",
                table: "listings",
                column: "city");

            migrationBuilder.CreateIndex(
                name: "IX_listings_latitude_longitude",
                table: "listings",
                columns: new[] { "latitude", "longitude" });

            migrationBuilder.CreateIndex(
                name: "IX_listings_listing_type",
                table: "listings",
                column: "listing_type");

            migrationBuilder.CreateIndex(
                name: "IX_listings_slug",
                table: "listings",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_listings_state",
                table: "listings",
                column: "state");

            migrationBuilder.CreateIndex(
                name: "IX_listings_status",
                table: "listings",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_listings_vendor_id",
                table: "listings",
                column: "vendor_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_created_at",
                table: "notifications",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_notification_type",
                table: "notifications",
                column: "notification_type");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_user_id",
                table: "notifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_user_id_is_read",
                table: "notifications",
                columns: new[] { "user_id", "is_read" });

            migrationBuilder.CreateIndex(
                name: "IX_otp_verifications_expires_at",
                table: "otp_verifications",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "IX_otp_verifications_mobile_or_email",
                table: "otp_verifications",
                column: "mobile_or_email");

            migrationBuilder.CreateIndex(
                name: "IX_otp_verifications_mobile_or_email_otp_code_is_used",
                table: "otp_verifications",
                columns: new[] { "mobile_or_email", "otp_code", "is_used" });

            migrationBuilder.CreateIndex(
                name: "IX_payments_booking_id",
                table: "payments",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_created_at",
                table: "payments",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_payments_gateway_transaction_id",
                table: "payments",
                column: "gateway_transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_payment_reference",
                table: "payments",
                column: "payment_reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payments_status",
                table: "payments",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_payouts_booking_id",
                table: "payouts",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_payouts_created_at",
                table: "payouts",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_payouts_status",
                table: "payouts",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_payouts_vendor_id",
                table: "payouts",
                column: "vendor_id");

            migrationBuilder.CreateIndex(
                name: "IX_payouts_VendorProfileId",
                table: "payouts",
                column: "VendorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_platform_settings_category",
                table: "platform_settings",
                column: "category");

            migrationBuilder.CreateIndex(
                name: "IX_platform_settings_is_public",
                table: "platform_settings",
                column: "is_public");

            migrationBuilder.CreateIndex(
                name: "IX_platform_settings_setting_key",
                table: "platform_settings",
                column: "setting_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_review_helpfuls_review_id",
                table: "review_helpfuls",
                column: "review_id");

            migrationBuilder.CreateIndex(
                name: "IX_review_helpfuls_review_id_user_id",
                table: "review_helpfuls",
                columns: new[] { "review_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_review_helpfuls_user_id",
                table: "review_helpfuls",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_booking_id",
                table: "reviews",
                column: "booking_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reviews_created_at",
                table: "reviews",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_customer_id",
                table: "reviews",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_is_published",
                table: "reviews",
                column: "is_published");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_listing_id",
                table: "reviews",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_rating",
                table: "reviews",
                column: "rating");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_vendor_id",
                table: "reviews",
                column: "vendor_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_VendorProfileId",
                table: "reviews",
                column: "VendorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_saved_listings_listing_id",
                table: "saved_listings",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "IX_saved_listings_user_id",
                table: "saved_listings",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_saved_listings_user_id_listing_id",
                table: "saved_listings",
                columns: new[] { "user_id", "listing_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_service_packages_listing_id",
                table: "service_packages",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_packages_listing_id_is_active",
                table: "service_packages",
                columns: new[] { "listing_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_sessions_expires_at",
                table: "sessions",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_refresh_token",
                table: "sessions",
                column: "refresh_token");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_token",
                table: "sessions",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sessions_user_id",
                table: "sessions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_profiles_user_id",
                table: "user_profiles",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_is_active",
                table: "users",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_users_mobile",
                table: "users",
                column: "mobile",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_role",
                table: "users",
                column: "role");

            migrationBuilder.CreateIndex(
                name: "IX_vendor_profiles_business_type",
                table: "vendor_profiles",
                column: "business_type");

            migrationBuilder.CreateIndex(
                name: "IX_vendor_profiles_city",
                table: "vendor_profiles",
                column: "city");

            migrationBuilder.CreateIndex(
                name: "IX_vendor_profiles_is_verified",
                table: "vendor_profiles",
                column: "is_verified");

            migrationBuilder.CreateIndex(
                name: "IX_vendor_profiles_latitude_longitude",
                table: "vendor_profiles",
                columns: new[] { "latitude", "longitude" });

            migrationBuilder.CreateIndex(
                name: "IX_vendor_profiles_state",
                table: "vendor_profiles",
                column: "state");

            migrationBuilder.CreateIndex(
                name: "IX_vendor_profiles_user_id",
                table: "vendor_profiles",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "booking_guests");

            migrationBuilder.DropTable(
                name: "booking_timelines");

            migrationBuilder.DropTable(
                name: "listing_amenities");

            migrationBuilder.DropTable(
                name: "listing_availability");

            migrationBuilder.DropTable(
                name: "listing_images");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "otp_verifications");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "payouts");

            migrationBuilder.DropTable(
                name: "platform_settings");

            migrationBuilder.DropTable(
                name: "review_helpfuls");

            migrationBuilder.DropTable(
                name: "saved_listings");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "user_profiles");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "service_packages");

            migrationBuilder.DropTable(
                name: "listings");

            migrationBuilder.DropTable(
                name: "vendor_profiles");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropSequence(
                name: "BookingReferenceSeq");
        }
    }
}
