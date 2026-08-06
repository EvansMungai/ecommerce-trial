using Catalog.Domain.Enums;
using Ecommerce.Shared;

namespace Catalog.Domain.Entities;

public class Product : BaseEntity, IAuditSupported, ISoftDeleted
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool ShowOnHomePage { get; private set; }
    public bool Published { get; private set; }

    #region Inventory Management
    public string Sku { get; private set; }
    public int StockQuantity { get; private set; }
    public int MinStockQuantity { get; private set; }
    public int LowStockActivityId { get; private set; }
    public LowStockActivity LowStockActivity
    {
        get => (LowStockActivity)LowStockActivityId;
        set => LowStockActivityId = (int)value;
    }
    public int OrderMinimumQuantity { get; private set; }
    public int OrderMaximumQuantity { get; private set; }
    #endregion

    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; set; }
    public bool Deleted { get; set; }

    private Product() { }
    public Product(string name, string description, string sku, int minStockQuantity, int lowStockActivityId)
    {
        Name = name;
        Description = description;
        ShowOnHomePage = true;
        Published = true;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
        Deleted = false;
        Sku = sku;
        StockQuantity = 0;
        MinStockQuantity = minStockQuantity;
        LowStockActivityId = lowStockActivityId;
    }

    public void UpdateProductDetails(string newName, string description, string sku, int minStockQuantity)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Product name is required", nameof(Name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(Description));

        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("Sku is required.", nameof(Sku));

        Name = newName;
        Description = description;
        Sku = sku;
        MinStockQuantity = minStockQuantity;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void RestockProduct(int stockQuantity)
    {
        StockQuantity += stockQuantity;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void DeductStock(int quantity)
    {
        StockQuantity -= quantity;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void SetProductStockRules(int minStockQuantity, int lowStockQuantityId)
    {
        MinStockQuantity = minStockQuantity;
        LowStockActivityId = LowStockActivityId;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void SetProductOrderRules(int minOrderQuantity, int maxOrderQuantity)
    {
        OrderMinimumQuantity = minOrderQuantity;
        OrderMaximumQuantity = maxOrderQuantity;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void ToggleProductHomepageVisibility(bool showOnHomePage)
    {
        ShowOnHomePage = showOnHomePage;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void ToggleProductVisibility(bool published)
    {
        Published = published;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Delete(int productId)
    {
        if (Deleted) return;
        Deleted = true;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
