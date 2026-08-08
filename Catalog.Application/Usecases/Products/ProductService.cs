using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Ecommerce.Shared.Events;

namespace Catalog.Application.Usecases.Products;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepo;
    private readonly IRepository<ProductCategory> _productCategoryRepo;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IRepository<Product> productRepo, IRepository<ProductCategory> productCategoryRepo, IUnitOfWork unitOfWork)
    {
        _productRepo = productRepo;
        _productCategoryRepo = productCategoryRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> CreateProduct(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        Product product = new Product(request.Name, request.Description, request.Sku, request.MinStockQuantity, request.LowStockActivityId);
        _productRepo.AddAsync(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Id;
    }

    public async Task<ProductResponse?> GetProduct(int id, CancellationToken cancellationToken = default)
    {
        Product? product = await _productRepo.GetSingleAsync(id, cancellationToken);
        if (product is null || product.Deleted)
            return null;

        return new ProductResponse(product.Name, product.Description, product.Sku, product.MinStockQuantity, product.StockQuantity);
    }

    public async Task<IEnumerable<ProductResponse>> GetProducts(CancellationToken cancellationToken = default)
    {
        IEnumerable<Product> products = await _productRepo.GetAllAsync(cancellationToken);
        return products.Where(p => !p.Deleted).Select(p => new ProductResponse(p.Name, p.Description, p.Sku, p.MinStockQuantity, p.StockQuantity)).ToList();
    }

    public async Task RemoveProduct(int id, CancellationToken cancellationToken = default)
    {
        Product? product = await _productRepo.GetSingleAsync(id, cancellationToken);
        if (product is null)
            throw new KeyNotFoundException("Product not found");

        product.Delete(id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RestockProduct(int stockQuantity, int id, CancellationToken cancellationToken = default)
    {
        Product? product = await _productRepo.GetSingleAsync(id, cancellationToken);
        if (product is null)
            throw new KeyNotFoundException("Product not found");

        product.RestockProduct(stockQuantity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeductStock(OrderCreatedEvent orderEvent, CancellationToken cancellationToken = default)
    {
        List<int> productIds = orderEvent.Items.Select(i => i.ProductId).ToList();
        IEnumerable<Product?> products = await _productRepo.GetFilteredAsync(p => productIds.Contains(p.Id), cancellationToken);

        foreach(var item in orderEvent.Items)
        {
            Product? product = products.FirstOrDefault(p => p is not null && p.Id ==  item.ProductId);
            if (product is null)
                throw new InvalidOperationException($"Product with Id {item.ProductId} was not found");

            product.DeductStock(item.Quantity);
        }
        //await _unitOfWork.SaveChangesAsync(cancellationToken);
}

    public async Task UpdateProduct(CreateProductRequest request, int id, CancellationToken cancellationToken = default)
    {
        Product? product = await _productRepo.GetSingleAsync(id, cancellationToken);
        if (product is null)
            throw new KeyNotFoundException("Product not found.");

        product.UpdateProductDetails(request.Name, request.Description, request.Sku, request.MinStockQuantity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
    public async Task AssignCategoryToProduct(AssignCategoryToProductRequest request, int id, CancellationToken cancellationToken = default)
    {
        Product? product = await _productRepo.GetSingleAsync(id, cancellationToken);
        if (product is null)
            throw new KeyNotFoundException("Product not found.");

        ProductCategory mapping = new ProductCategory(product.Id, request.CategoryId);
        _productCategoryRepo.AddAsync(mapping);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateProductOrderRules(SetProductOrderRulesRequest request, int id, CancellationToken cancellationToken = default)
    {
        Product? product = await _productRepo.GetSingleAsync(id, cancellationToken);
        if (product is null)
            throw new KeyNotFoundException("Product not found.");

        product.SetProductOrderRules(request.MinOrderQuantity, request.MaxOrderQuantity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateProductStockRules(SetProductStockRulesRequest request, int id, CancellationToken cancellationToken = default)
    {
        Product? product = await _productRepo.GetSingleAsync(id, cancellationToken);
        if (product is null)
            throw new KeyNotFoundException("Product not found.");

        product.SetProductStockRules(request.MinStockQuantity, request.LowStockActivityId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
