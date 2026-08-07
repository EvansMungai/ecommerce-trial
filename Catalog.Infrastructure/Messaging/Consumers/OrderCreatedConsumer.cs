using Catalog.Application.Interfaces;
using Catalog.Application.Usecases.Products;
using Ecommerce.Shared.Events;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;

namespace Catalog.Infrastructure.Messaging.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly IProductService _productService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDistributedCache _cache;

    public OrderCreatedConsumer(IProductService productService, IUnitOfWork unitOfWork, IDistributedCache cache)
    {
        _productService = productService;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        OrderCreatedEvent orderEvent = context.Message;

        if (await IsDuplicateAndTrackAsync(context))
            return;
        try
        {
            await _productService.DeductStock(orderEvent, context.CancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var cachekey = $"idempotency: order_created: {context.MessageId}";
            await _cache.RemoveAsync(cachekey, context.CancellationToken);
            throw;
        }
    }
    private async Task<bool> IsDuplicateAndTrackAsync(ConsumeContext<OrderCreatedEvent> context)
    {
        string? messageId = context.MessageId?.ToString();
        if (string.IsNullOrEmpty(messageId)) return false;

        string cachekey = $"idempotency:order_created:{messageId}";
        var isAlreadyProcessed = await _cache.GetStringAsync(cachekey, context.CancellationToken);
        if (isAlreadyProcessed is not null)
            return true;

        DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
        };
        await _cache.SetStringAsync(cachekey, "processed", context.CancellationToken);
        return false;
    }
}
