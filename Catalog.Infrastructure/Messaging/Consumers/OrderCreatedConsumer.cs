using Catalog.Application.Interfaces;
using Catalog.Application.Usecases.Products;
using Ecommerce.Shared.Events;
using MassTransit;

namespace Catalog.Infrastructure.Messaging.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly IProductService _productService;
    private readonly IUnitOfWork _unitOfWork;

    public OrderCreatedConsumer(IProductService productService, IUnitOfWork unitOfWork)
    {
        _productService = productService;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        OrderCreatedEvent orderEvent = context.Message;
        //Guid trackingId = orderEvent.OrderId;
        await _productService.DeductStock(orderEvent, context.CancellationToken);
        await _unitOfWork.SaveChangesAsync();
    }
}
