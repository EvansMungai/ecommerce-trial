using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces;

public interface IQueuePublisher
{
    Task PublishCategoryCreatedAsync(Category category, CancellationToken cancellationToken);
}
