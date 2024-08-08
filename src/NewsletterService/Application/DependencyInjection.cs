using Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(
            e =>
            {
                e.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                e.AddBehavior(typeof(IPipelineBehavior<,>), typeof(CacheQueryPipelineBehavior<,>));
                e.AddBehavior(typeof(IPipelineBehavior<,>), typeof(InvalidateCacheCommandPipelineBehavior<,>));
            });

        return services;
    }
}
