using Microsoft.Extensions.DependencyInjection;
using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Handlers.Admin.Bookings;
using SambhramaBook.Application.Handlers.Admin.Listings;
using SambhramaBook.Application.Handlers.Admin.Payouts;
using SambhramaBook.Application.Handlers.Admin.Reviews;
using SambhramaBook.Application.Handlers.Admin.Settings;
using SambhramaBook.Application.Handlers.Admin.Users;
using SambhramaBook.Application.Handlers.Admin.Vendors;
using SambhramaBook.Application.Handlers.Auth;
using SambhramaBook.Application.Handlers.Booking;
using SambhramaBook.Application.Handlers.FileUpload;
using SambhramaBook.Application.Handlers.Notifications;
using SambhramaBook.Application.Handlers.Payment;
using SambhramaBook.Application.Handlers.Reviews;
using SambhramaBook.Application.Handlers.SavedVenues;
using SambhramaBook.Application.Handlers.UserProfile;
using SambhramaBook.Application.Handlers.Vendor;
using SambhramaBook.Application.Handlers.VendorAvailability;
using SambhramaBook.Application.Handlers.VendorBookings;
using SambhramaBook.Application.Handlers.VendorListings;
using SambhramaBook.Application.Handlers.VendorSettings;

namespace SambhramaBook.Api.Configuration;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterApiServices(this IServiceCollection services)
    {
        // Note: All queries are registered in SambhramaBook.Infrastructure.SambhramaBookServicesRegistrar
        // IHallQueries, ISearchQueries, IServiceQueries, IReviewQueries, INotificationQueries,
        // ISavedListingQueries, IAdminQueries, IVendorQueries are all registered there
        return services
            .RegisterApplicationHandlers()
            .RegisterHandlers("SambhramaBook.Application"); // Auto-register IQueryHandler implementations
    }

    private static IServiceCollection RegisterApplicationHandlers(this IServiceCollection services)
    {
        // Handlers that don't implement IQueryHandler interface are registered manually
        // Handlers implementing IQueryHandler are auto-registered via HandlersRegistration
        return services
            .AddScoped<SendOtpHandler>()
            .AddScoped<VerifyOtpHandler>()
            .AddScoped<GetCurrentUserHandler>()
            .AddScoped<LogoutHandler>()
            .AddScoped<RefreshTokenHandler>()
            .AddScoped<CompleteOnboardingHandler>()
            .AddScoped<GetOnboardingStatusHandler>()
            .AddScoped<CreateBookingHandler>()
            .AddScoped<CancelBookingHandler>()
            .AddScoped<RescheduleBookingHandler>()
            .AddScoped<ProcessPaymentHandler>()
            .AddScoped<ApproveBookingHandler>()
            .AddScoped<RejectBookingHandler>()
            .AddScoped<CreateVendorBookingHandler>()
            .AddScoped<InitiatePaymentHandler>()
            .AddScoped<VerifyPaymentHandler>()
            .AddScoped<ProcessRefundHandler>()
            .AddScoped<UpdateUserProfileHandler>()
            .AddScoped<ChangePasswordHandler>()
            .AddScoped<SaveListingHandler>()
            .AddScoped<RemoveSavedListingHandler>()
            .AddScoped<MarkNotificationReadHandler>()
            .AddScoped<MarkAllNotificationsReadHandler>()
            .AddScoped<CreateVendorListingHandler>()
            .AddScoped<UpdateVendorListingHandler>()
            .AddScoped<DeleteVendorListingHandler>()
            .AddScoped<UpdateListingStatusHandler>()
            .AddScoped<UploadListingImagesHandler>()
            .AddScoped<UpdateVendorAvailabilityHandler>()
            .AddScoped<BlockDatesHandler>()
            .AddScoped<UnblockDatesHandler>()
            .AddScoped<UpdateVendorSettingsHandler>()
            .AddScoped<CreateReviewHandler>()
            .AddScoped<MarkReviewHelpfulHandler>()
            .AddScoped<AddVendorResponseHandler>()
            .AddScoped<UploadImageHandler>()
            .AddScoped<UploadMultipleImagesHandler>()
            .AddScoped<DeleteUploadedFileHandler>()
            .AddScoped<GenerateUploadTokenHandler>()
            .AddScoped<UpdateUserStatusHandler>()
            .AddScoped<DeleteUserHandler>()
            .AddScoped<VerifyVendorHandler>()
            .AddScoped<UpdateVendorStatusHandler>()
            .AddScoped<ApproveListingHandler>()
            .AddScoped<RejectListingHandler>()
            .AddScoped<RequestListingChangesHandler>()
            .AddScoped<UpdateBookingStatusHandler>()
            .AddScoped<ProcessPayoutHandler>()
            .AddScoped<UpdatePayoutStatusHandler>()
            .AddScoped<PublishReviewHandler>()
            .AddScoped<DeleteReviewHandler>()
            .AddScoped<UpdateSettingHandler>()
            .AddScoped<CreateSettingHandler>();
    }
}

