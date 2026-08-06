using Order.Infrastructure;

namespace Order.API;

public static class DependencyRegistrar
{
    public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.RegisterInfrastructureServices(config);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
