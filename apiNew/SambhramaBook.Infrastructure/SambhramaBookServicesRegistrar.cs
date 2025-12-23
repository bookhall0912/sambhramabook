using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Application.Configuration;
using SambhramaBook.Application.Services;
using SambhramaBook.Infrastructure.Repository;
using SambhramaBook.Infrastructure.Services;
using Microsoft.AspNetCore.Http;

namespace SambhramaBook.Infrastructure;

public static class SambhramaBookServicesRegistrar
{
    public static IServiceCollection RegisterSambhramaBookServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .RegisterInfrastructureServices(configuration)
            .RegisterApplicationServices();
    }

    private static IServiceCollection RegisterInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<SambhramaBookDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Configuration - JWT settings are configured in Program.cs, so we can skip here
        // services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        
        // HttpContextAccessor - AddHttpContextAccessor extension method requires Microsoft.Extensions.DependencyInjection package
        // Since it's not available, we'll register it manually if needed
        // Note: This should be handled in the API project where ASP.NET Core packages are available

        // Repositories
        return services
            .RegisterRepositories()
            .RegisterQueries()
            .RegisterInfrastructureServices();
    }

    private static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IBlobStorageService, BlobStorageService>()
            .AddScoped<IOtpService, TwilioOtpService>()
            .AddScoped<IPasswordHasher, PasswordHasher>();
    }

    private static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IVendorProfileRepository, VendorProfileRepository>()
            .AddScoped<IListingRepository, ListingRepository>()
            .AddScoped<IListingAvailabilityRepository, ListingAvailabilityRepository>()
            .AddScoped<IBookingRepository, BookingRepository>()
            .AddScoped<IPaymentRepository, PaymentRepository>()
            .AddScoped<IReviewRepository, ReviewRepository>()
            .AddScoped<INotificationRepository, NotificationRepository>()
            .AddScoped<ISavedListingRepository, SavedListingRepository>()
            .AddScoped<ISessionRepository, SessionRepository>()
            .AddScoped<IOtpVerificationRepository, OtpVerificationRepository>()
            .AddScoped<IPayoutRepository, PayoutRepository>()
            .AddScoped<IPlatformSettingRepository, PlatformSettingRepository>();
    }

    private static IServiceCollection RegisterQueries(this IServiceCollection services)
    {
        return services
            .AddScoped<SambhramaBook.Application.Queries.IHallQueries, Queries.HallQueries>()
            .AddScoped<SambhramaBook.Application.Queries.ISearchQueries, Queries.SearchQueries>()
            .AddScoped<SambhramaBook.Application.Queries.IServiceQueries, Queries.ServiceQueries>()
            .AddScoped<SambhramaBook.Application.Queries.IReviewQueries, Queries.ReviewQueries>()
            .AddScoped<SambhramaBook.Application.Queries.INotificationQueries, Queries.NotificationQueries>()
            .AddScoped<SambhramaBook.Application.Queries.ISavedListingQueries, Queries.SavedListingQueries>()
            .AddScoped<SambhramaBook.Application.Queries.IAdminQueries, Queries.AdminQueries>()
            .AddScoped<SambhramaBook.Application.Queries.IVendorQueries, Queries.VendorQueries>();
    }

    private static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        return ApplicationServicesRegistrar.RegisterApplicationServices(services);
    }
}
