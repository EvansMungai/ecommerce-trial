using Ecommerce.Shared.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Order.Application.Interfaces;

namespace Order.Infrastructure.Messaging;

public class MassTransitPublisher : IQueuePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<MassTransitPublisher> _logger;

    public MassTransitPublisher(IPublishEndpoint publishEndpoint, ILogger<MassTransitPublisher> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task PublishOrderCreatedAsync(OrderCreatedEvent orderCreatedEvent, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(orderCreatedEvent, cancellationToken);
    }
}
