using Catalog.Application.Dtos;
using Ecommerce.Shared.Events;

namespace Catalog.Application.Usecases.Products;

public interface IProductService
{
    Task<IEnumerable<ProductResponse>> GetProducts(CancellationToken cancellationToken = default);
    Task<ProductResponse?> GetProduct(int id, CancellationToken cancellationToken = default);
    Task<int> CreateProduct(CreateProductRequest request, CancellationToken cancellationToken = default);
    Task UpdateProduct(CreateProductRequest request, int id, CancellationToken cancellationToken = default);
    Task RestockProduct(int stockQuantity, int id, CancellationToken cancellationToken = default);
    Task DeductStock(OrderCreatedEvent orderEvent, CancellationToken cancellationToken = default);
    Task AssignCategoryToProduct(AssignCategoryToProductRequest request, int id, CancellationToken cancellationToken = default);
    Task UpdateProductStockRules(SetProductStockRulesRequest request, int id, CancellationToken cancellationToken = default);
    Task UpdateProductOrderRules(SetProductOrderRulesRequest request, int id, CancellationToken cancellationToken = default);
    Task RemoveProduct(int id, CancellationToken cancellationToken = default);
}
