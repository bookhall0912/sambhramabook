using Microsoft.EntityFrameworkCore;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure;

public class SambhramaBookDbContext : DbContext
{
    public const string BookingReferenceSequenceName = "BookingReferenceSeq";

    public SambhramaBookDbContext()
    {
    }

    public SambhramaBookDbContext(DbContextOptions<SambhramaBookDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; init; }
    public virtual DbSet<UserProfile> UserProfiles { get; init; }
    public virtual DbSet<VendorProfile> VendorProfiles { get; init; }
    public virtual DbSet<ServiceCategory> ServiceCategories { get; init; }
    public virtual DbSet<Listing> Listings { get; init; }
    public virtual DbSet<ListingImage> ListingImages { get; init; }
    public virtual DbSet<ListingAmenity> ListingAmenities { get; init; }
    public virtual DbSet<ServicePackage> ServicePackages { get; init; }
    public virtual DbSet<ListingAvailability> ListingAvailabilities { get; init; }
    public virtual DbSet<Booking> Bookings { get; init; }
    public virtual DbSet<BookingGuest> BookingGuests { get; init; }
    public virtual DbSet<BookingTimeline> BookingTimelines { get; init; }
    public virtual DbSet<Payment> Payments { get; init; }
    public virtual DbSet<Review> Reviews { get; init; }
    public virtual DbSet<ReviewHelpful> ReviewHelpfuls { get; init; }
    public virtual DbSet<Notification> Notifications { get; init; }
    public virtual DbSet<SavedListing> SavedListings { get; init; }
    public virtual DbSet<Session> Sessions { get; init; }
    public virtual DbSet<OtpVerification> OtpVerifications { get; init; }
    public virtual DbSet<Payout> Payouts { get; init; }
    public virtual DbSet<PlatformSetting> PlatformSettings { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configuration will be done via dependency injection
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all entity configurations from assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SambhramaBookDbContext).Assembly);

        // Global query filters
        modelBuilder.Entity<User>().HasQueryFilter(u => u.DeletedAt == null);
        modelBuilder.Entity<Listing>().HasQueryFilter(l => l.DeletedAt == null);

        // Define sequence for BookingReference
        modelBuilder.HasSequence<int>(BookingReferenceSequenceName)
            .StartsAt(1000)
            .IncrementsBy(1)
            .HasMin(1000)
            .HasMax(999999)
            .IsCyclic();

        base.OnModelCreating(modelBuilder);
    }
}

