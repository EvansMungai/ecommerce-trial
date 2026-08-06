using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.Messaging;

public class MassTransitPublisher : IQueuePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<MassTransitPublisher> _logger;

    public MassTransitPublisher(IPublishEndpoint publishEndpoint, ILogger<MassTransitPublisher> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task PublishCategoryCreatedAsync(Category category, CancellationToken cancellationToken)
    {
        CategoryCreatedEvent message = new CategoryCreatedEvent(category.Id, DateTime.UtcNow);

        await _publishEndpoint.Publish(message, cancellationToken);
        _logger.LogInformation($"Published CategoryCreatedEvent for {category.Id}");
    }
}
