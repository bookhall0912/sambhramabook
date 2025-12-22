using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Application.Common;
using SambhramaBook.Application.Configuration;
using SambhramaBook.Application.Services;
using SambhramaBook.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;

namespace SambhramaBook.Infrastructure;

public static class SambhramaBookServicesRegistrar
{
    public static IServiceCollection RegisterSambhramaBookServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<SambhramaBookDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Configuration
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // Common Services
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddHttpContextAccessor();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVendorProfileRepository, VendorProfileRepository>();
        services.AddScoped<IListingRepository, ListingRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<ISavedListingRepository, SavedListingRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IOtpVerificationRepository, OtpVerificationRepository>();
        services.AddScoped<IPayoutRepository, PayoutRepository>();
        services.AddScoped<IPlatformSettingRepository, PlatformSettingRepository>();

        return services;
    }
}

