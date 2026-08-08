using Ecommerce.Shared.Events;
using Order.Application.Dtos;

namespace Order.Application.Usecases;

public interface IOrderService
{
    Task<OrderResponse> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken = default);
    Task<OrderResponse> GetOrderByGuidAsync(GetOrderByGuidRequest request, CancellationToken cancellationToken = default);
    Task<OrderResponse> GetOrderByPhoneNumberAsync(string phonenumber, CancellationToken cancellationToken = default);
    Task<Guid> CreateOrder(CreateOrderRequest request, CancellationToken cancellationToken = default);
    Task UpdateOrderDetails(CreateOrderRequest order, int id, CancellationToken cancellationToken = default);
    Task UpdateOrderStatus(StockDeductedEvent request, int id, CancellationToken cancellationToken = default);
    Task RemoveOrderAsync(int id, CancellationToken cancellationToken = default);
}
