namespace Order.Application.Dtos;

public record AddOrderItemRequest(int ProductId, int Quantity, decimal UnitPrice);
public record CreateOrderRequest(string Phonenumber, List<AddOrderItemRequest> items);
public record GetOrderByGuidRequest(Guid orderGuid);
public record OrderResponse(Guid Guid, string Phonenumber, string OrderStatus, string PaymentStatus, DateTime CreatedAt, DateTime UpdatedAt);
public record UpdateOrderStatusRequest(Guid OrderGuid, string Status);