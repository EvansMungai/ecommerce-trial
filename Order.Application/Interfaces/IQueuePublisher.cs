using Ecommerce.Shared.Events;

namespace Order.Application.Interfaces;

public interface IQueuePublisher
{
    Task PublishOrderCreatedAsync(OrderCreatedEvent orderCreatedEvent, CancellationToken cancellationToken);
}
