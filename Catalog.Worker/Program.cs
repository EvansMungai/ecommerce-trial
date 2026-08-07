using Catalog.Infrastructure;
using Catalog.Worker.Consumers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.RegisterInfrastructureServices(builder.Configuration, masstransit =>
{
    masstransit.AddConsumer<OrderCreatedConsumer>();
});

var host = builder.Build();
host.Run();
