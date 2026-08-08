using Ecommerce.Shared.Events;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Order.Application.Interfaces;
using Order.Application.Usecases;

namespace Order.Worker.Consumers;

public class StockDeductedConsumer : IConsumer<StockDeductedEvent>
{
    private readonly IOrderService _orderService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDistributedCache _cache;

    public StockDeductedConsumer(IOrderService orderService, IUnitOfWork unitOfWork, IDistributedCache cache)
    {
        _orderService = orderService;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task Consume(ConsumeContext<StockDeductedEvent> context)
    {
        StockDeductedEvent stockDeductedEvent = context.Message;
        if (await IsDuplicateAndTrackAsync(context))
            return;

        await _orderService.UpdateOrderStatus(stockDeductedEvent, stockDeductedEvent.Id);
        await _unitOfWork.SaveChangesAsync(context.CancellationToken); ;
    }
    private async Task<bool> IsDuplicateAndTrackAsync(ConsumeContext<StockDeductedEvent> context)
    {
        string? messageId = context.MessageId?.ToString();
        if (string.IsNullOrEmpty(messageId)) return false;

        string cachekey = $"idempotency:stock_deducted:{messageId}";
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
