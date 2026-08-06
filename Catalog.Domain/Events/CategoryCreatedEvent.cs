namespace Catalog.Domain.Events;

public record CategoryCreatedEvent(int CategoryId, DateTime CreatedAt);
