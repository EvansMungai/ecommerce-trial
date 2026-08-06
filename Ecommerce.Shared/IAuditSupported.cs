namespace Ecommerce.Shared;

public interface IAuditSupported
{
    DateTime CreatedAtUtc { get; init; }
    DateTime UpdatedAtUtc { get; set; }
}
