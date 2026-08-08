namespace Ecommerce.Shared.Events;

public record OrderCreatedEvent(Guid OrderGuid, int Id, List<OrderItemEventDto> Items, DateTime CreatedAt);
public record OrderItemEventDto(int ProductId, int Quantity);
