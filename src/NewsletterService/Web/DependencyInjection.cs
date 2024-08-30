using System.Reflection;
using Web.Core.Interfaces;

namespace Web;

public static class DependencyInjection
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        foreach (var endpointType in Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(IEndpoint)
            .IsAssignableFrom(t))) ((IEndpoint)Activator.CreateInstance(endpointType)!).Map(app);

        return app;
    }
}
