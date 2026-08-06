using Ecommerce.Shared;
using Order.Domain.Enums;

namespace Order.Domain.Entities;

public class OrderDomain : BaseEntity, IAuditSupported, ISoftDeleted
{
    public Guid OrderGuid { get; set; }
    public string Phonenumber{ get; set; }
    public int OrderStatusId { get; set; }
    public OrderStatus OrderStatus
    {
        get => (OrderStatus)OrderStatusId;
        set => OrderStatusId = (int)value;
    }
    public int PaymentStatusId { get; set; }
    public PaymentStatus PaymentStatus
    {
        get => (PaymentStatus)PaymentStatusId;
        set => PaymentStatusId = (int)value;
    }
    public decimal Total { get; private set; }
    public List<OrderItem> OrderItems { get; private set; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; set; }
    public bool Deleted { get; set; }

    private OrderDomain() { }
    public OrderDomain(string phonenumber, List<OrderItem> items)
    {
        if(items is null || !items.Any())
            throw new ArgumentException("Order items cannot be empty", nameof(items));

        OrderGuid = Guid.NewGuid();
        Phonenumber = phonenumber;
        OrderStatusId = 10;
        PaymentStatusId = 10;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
        Deleted = false;
        OrderItems = items;
        Total = OrderItems.Sum(i => i.TotalPrice);
    }
    public void MarkAsProcessing()
    {
        if (OrderStatus != OrderStatus.Pending)
            throw new InvalidOperationException("Only pending orders can be processed");

        OrderStatusId = 20;            
    }
    public void MarkAsCompleted()
    {
        if (OrderStatus != OrderStatus.Processing)
            throw new InvalidOperationException("Only processing orders can be marked as completed");

        OrderStatusId = 30;
    }

    public void MarkAsCancelled()
    {
        if (OrderStatus != OrderStatus.Processing)
            throw new InvalidOperationException("Only processing orders can be marked as cancelled");

        OrderStatusId = 40;
    }
}
