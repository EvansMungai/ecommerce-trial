using Order.Infrastructure;
using Order.Worker.Consumers;

var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();
builder.Services.RegisterInfrastructureServices(builder.Configuration, masstransit =>
{
    masstransit.AddConsumer<StockDeductedConsumer>();
});

var host = builder.Build();
host.Run();
