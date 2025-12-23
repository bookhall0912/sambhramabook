using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SambhramaBook.Application.Common.Handlers;

public static class HandlersRegistration
{
    public static IServiceCollection RegisterHandlers(this IServiceCollection services, string assemblyName)
    {
        Assembly assembly = AppDomain.CurrentDomain.GetAssemblies()
            .Single(a => a.GetName().Name == assemblyName);
        
        Type[] handlerInterfaceTypes = [typeof(IQueryHandler<>), typeof(IQueryHandler<,>)];
        
        foreach (Type handlerInterfaceType in handlerInterfaceTypes)
        {
            services.RegisterHandlers(assembly, handlerInterfaceType);
        }
        
        return services;
    }

    private static void RegisterHandlers(this IServiceCollection services, Assembly assembly, Type queryHandlerType)
    {
        var handlerTypes = assembly.GetTypes()
           .Where(t => !t.IsAbstract && !t.IsInterface)
           .Select(t => new
           {
               Implementation = t,
               SpecificInterface = t.GetInterfaces()
                   .FirstOrDefault(i => i != null &&
                   i != queryHandlerType &&
                   i.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == queryHandlerType))
           })
           .Where(x => x.SpecificInterface != null)
           .ToArray();

        foreach (var handler in handlerTypes)
        {
            services.AddScoped(handler.SpecificInterface!, handler.Implementation);
        }
    }
}

