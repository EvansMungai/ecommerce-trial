namespace Ecommerce.Shared.Events;

public record OrderCreatedEvent(Guid OrderId, List<OrderItemEventDto> Items, DateTime CreatedAt);
public record OrderItemEventDto(int ProductId, int Quantity);
