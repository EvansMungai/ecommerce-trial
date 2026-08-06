using Ecommerce.Shared;

namespace Order.Domain.Entities;

public class OrderItem : BaseEntity, IAuditSupported
{
    public int OrderId { get; private set; }
    public int ProductId { get; private set; }

    #region Quantity & Financials
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalPrice { get; private set; }
    #endregion

    public virtual OrderDomain Order { get; private set; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; set; }

    private OrderItem() { }

    public OrderItem(int productId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));

        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
        TotalPrice = UnitPrice * Quantity;
    }
}
