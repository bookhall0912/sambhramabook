using Microsoft.Extensions.DependencyInjection;
using SambhramaBook.Application.Common;
using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Services;

namespace SambhramaBook.Application.Configuration;

public static class ApplicationServicesRegistrar
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        return services
            .RegisterHandlers("SambhramaBook.Application")
            .RegisterApplicationCommonServices();
    }

    private static IServiceCollection RegisterApplicationCommonServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IDateTimeProvider, DateTimeProvider>()
            .AddScoped<IJwtTokenService, JwtTokenService>();
    }
}

