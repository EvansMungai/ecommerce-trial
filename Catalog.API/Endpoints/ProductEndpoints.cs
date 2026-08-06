using Catalog.Application.Dtos;
using Catalog.Application.Usecases.Products;

namespace Catalog.API.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api").WithTags("Product Management");
        group.MapGet("/products", async (IProductService service, CancellationToken ct) =>
        {
            IEnumerable<ProductResponse> products = await service.GetProducts(ct);
            return products is null || !products.Any() ? Results.NotFound("No products were found") : Results.Ok(products);
        });
        group.MapGet("/product/{id}", async (IProductService service, int id, CancellationToken ct) =>
        {
            ProductResponse? product = await service.GetProduct(id, ct);
            return product is null ? Results.NotFound("Product not found") : Results.Ok(product);
        });
        group.MapPost("/product", async (IProductService service, CreateProductRequest request, CancellationToken ct) =>
        {
            int productId = await service.CreateProduct(request, ct);
            return Results.Created("/api/product/{id}", new { id = productId });
        });
        group.MapPut("/product/{id}", async (IProductService service, CreateProductRequest request, int id, CancellationToken ct) =>
        {
            await service.UpdateProduct(request, id, ct);
            return Results.NoContent();
        });
        group.MapDelete("/product/{id}", async (IProductService services, int id, CancellationToken ct) =>
        {
            await services.RemoveProduct(id, ct);
            return Results.NoContent();
        });
        group.MapPut("/product-categorization/{id}", async (IProductService service, AssignCategoryToProductRequest request, int id, CancellationToken ct) =>
        {
            await service.AssignCategoryToProduct(request, id, ct);
            return Results.NoContent();
        });
        group.MapPut("/restock-product/{id}", async (IProductService services, RestockProductRequest request, int id, CancellationToken ct) =>
        {
            await services.RestockProduct(request.StockQuantity, id, ct);
            return Results.NoContent();
        });
    }
}
