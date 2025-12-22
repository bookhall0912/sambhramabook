using Microsoft.EntityFrameworkCore;
using SambhramaBook.Domain;

namespace SambhramaBook.Infrastructure;

public partial class SambhramaBookDbContext : DbContext
{
    public SambhramaBookDbContext()
    {
    }

    public SambhramaBookDbContext(DbContextOptions<SambhramaBookDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ServiceCategory> ServiceCategories { get; init; }
    public virtual DbSet<Service> Services { get; init; }
    public virtual DbSet<Amenity> Amenities { get; init; }
    public virtual DbSet<ServiceAmenity> ServiceAmenities { get; init; }
    public virtual DbSet<HallServiceDetail> HallServiceDetails { get; init; }
    public virtual DbSet<Review> Reviews { get; init; }
    public virtual DbSet<ServiceMedia> ServiceMedias { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceCategory>().HasQueryFilter(l => l.IsActive);
        modelBuilder.Entity<Amenity>().HasQueryFilter(a => a.IsActive);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SambhramaBookDbContext).Assembly);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
