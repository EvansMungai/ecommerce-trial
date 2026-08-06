using Order.Application.Dtos;
using Order.Application.Usecases;

namespace Order.API.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api").WithTags("Order Management");
        group.MapPost("/order", async (IOrderService service, CreateOrderRequest request, CancellationToken ct) =>
        {
            Guid orderGuid = await service.CreateOrder(request, ct);
            return Results.Created("/api/order/{id}", new {id =  orderGuid});
        });
        group.MapGet("/order{id:guid}", async (IOrderService service, Guid id, CancellationToken ct) =>
        {
            GetOrderByGuidRequest request = new GetOrderByGuidRequest(id);
            OrderResponse order = await service.GetOrderByGuidAsync(request, ct);
            return order is null ? Results.NotFound("No order wasfound") : Results.Ok(order);
        });
    }
}
