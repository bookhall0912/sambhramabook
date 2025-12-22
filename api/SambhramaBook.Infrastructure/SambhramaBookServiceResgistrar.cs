using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Queries;
using SambhramaBook.Infrastructure.Queries;

namespace SambhramaBook.Infrastructure;

public static class SambhramaBookServiceResgistrar
{
    public static IServiceCollection RegisterSambhramaBookServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.RegisterInfrastructureServices(configuration)
            .RegisterApplicationServices();
    }

    private static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDbContext<SambhramaBookDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PortalConnectionString")));
    }

    private static IServiceCollection RegisterApplicationServices(this IServiceCollection services) =>
        services.AddScoped<IServiceQueries, ServiceQueries>()
                .RegisterQueryHandlers("SambhramaBook.Application");
}
