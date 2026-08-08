using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Interfaces;
using Order.Application.Usecases;
using Order.Infrastructure.Messaging;
using Order.Infrastructure.Persistence;

namespace Order.Infrastructure;

public static class DependencyRegistrar
{
    public static void RegisterInfrastructureServices(this IServiceCollection services, IConfiguration config, Action<IBusRegistrationConfigurator>? configureConsumers = null)
    {
        services.AddDbContext<OrderDbContext>(options => options.UseNpgsql(config.GetConnectionString("Order")));

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<OrderDbContext>());

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IQueuePublisher, MassTransitPublisher>();

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
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "cache:6379";
                options.InstanceName = "OrderSvc:";
            });
        });
    }
}
