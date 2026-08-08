namespace Ecommerce.Shared.Events;

public record StockDeductedEvent(Guid OrderGuid, int Id, DateTime OccurredAt);