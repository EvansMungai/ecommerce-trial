using Catalog.Application.Interfaces;
using Catalog.Application.Usecases.Categories;
using Catalog.Application.Usecases.Products;
using Catalog.Infrastructure.Messaging;
using Catalog.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure;

public static class DependencyRegistrar
{
    public static void RegisterInfrastructureServices(this IServiceCollection services, IConfiguration config, Action<IBusRegistrationConfigurator>? configureConsumers = null)
    {
        services.AddDbContext<CatalogDbContext>(options => options.UseNpgsql(config.GetConnectionString("Catalog")));

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<CatalogDbContext>());

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();

        services.Configure<QueueSettings>(config.GetSection("QueueSettings"));
        services.AddMassTransit(x =>
        {
            configureConsumers?.Invoke(x);
            x.UsingRabbitMq((context, cfg) =>
            {
                if (config is null)
                    throw new InvalidOperationException("Configuration object is null in Add Infrastructure services");

                var section = config.GetSection("QueueSettings");
                if (!section.Exists())
                    throw new InvalidOperationException("QueueSettings section is missing in configuration");

                var queueSettings = section.Get<QueueSettings>();
                if (queueSettings == null)
                    throw new InvalidOperationException("QueueSettings could not be bound to QueueSettings class.");

                cfg.Host(queueSettings.Host, h =>
                {
                    h.Username(queueSettings.Username);
                    h.Password(queueSettings.Password);
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "cache:6379";
            options.InstanceName = "CatalogSvc:";
        });
    }
}
