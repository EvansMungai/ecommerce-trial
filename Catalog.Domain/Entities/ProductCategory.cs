using Ecommerce.Shared;

namespace Catalog.Domain.Entities;

public class ProductCategory : BaseEntity, IAuditSupported
{
    public int ProductId { get; private set; }
    public int CategoryId { get; private set; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; set; }

    public ProductCategory(int productId, int categoryId)
    {
        if (productId <= 0)
            throw new ArgumentException("Product Id must be greater than zero", nameof(productId));

        if (categoryId <= 0)
            throw new ArgumentException("Catgory Id must be greater than zero", nameof(categoryId));

        ProductId = productId;
        CategoryId = categoryId;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void ChangeProductCategory(int categoryId)
    {
        if (categoryId <= 0)
            throw new ArgumentException("Category Id must be greater than zero", nameof(categoryId));

        if (CategoryId == categoryId)
            return;

        CategoryId = categoryId;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
