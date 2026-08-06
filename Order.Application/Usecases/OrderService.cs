using Ecommerce.Shared.Events;
using Order.Application.Dtos;
using Order.Application.Interfaces;
using Order.Domain.Entities;

namespace Order.Application.Usecases;

public class OrderService : IOrderService
{
    private readonly IRepository<OrderDomain> _orderRepo;
    private readonly IRepository<OrderItem> _orderItemRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQueuePublisher _publishEndpoint;

    public OrderService(IRepository<OrderDomain> orderRepo, IRepository<OrderItem> orderItemRepo, IUnitOfWork unitOfWork, IQueuePublisher publishEndpoint)
    {
        _orderRepo = orderRepo;
        _orderItemRepo = orderItemRepo;
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Guid> CreateOrder(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        List<OrderItem> orderItems = request.items.Select(item => new OrderItem(item.ProductId, item.Quantity, item.UnitPrice)).ToList();
        OrderDomain order = new OrderDomain(request.Phonenumber, orderItems);

        _orderRepo.AddAsync(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        List<OrderItemEventDto> eventItems = orderItems.Select(item => new OrderItemEventDto(item.ProductId, item.Quantity)).ToList();
        OrderCreatedEvent orderCreatedEvent = new OrderCreatedEvent(order.OrderGuid, eventItems, DateTime.UtcNow);
        await _publishEndpoint.PublishOrderCreatedAsync(orderCreatedEvent, cancellationToken);

        return order.OrderGuid;
    }

    public Task<OrderResponse> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    public async Task<OrderResponse?> GetOrderByGuidAsync(GetOrderByGuidRequest request, CancellationToken cancellationToken = default)
    {
        OrderDomain? order = await _orderRepo.GetSingleWithIncludeAsync(o => o.OrderGuid == request.orderGuid && !o.Deleted, cancellationToken, o => o.OrderItems);
        if (order is null)
            return null;

        return new OrderResponse(order.OrderGuid, order.Phonenumber, order.OrderStatus.ToString(), order.PaymentStatus.ToString(), order.CreatedAtUtc, order.UpdatedAtUtc);
    }

    public Task<OrderResponse> GetOrderByPhoneNumberAsync(string phonenumber, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RemoveOrderAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateOrderDetails(CreateOrderRequest order, int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
