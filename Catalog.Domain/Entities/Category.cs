using Ecommerce.Shared;

namespace Catalog.Domain.Entities;

public class Category : BaseEntity, IAuditSupported, ISoftDeleted
{

    public string Name { get; private set; }
    public int? ParentCategoryId { get; private set; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; set; }
    public bool Deleted { get; set; }
    public Category(string name, int? parentCategoryId = null)
    {
        Name = name;
        ParentCategoryId = parentCategoryId;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
        Deleted = false;
    }

    private Category() { }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Catgory name cannot be empty", nameof(newName));

        if (Deleted)
            throw new InvalidOperationException("Cannot rename deleted category");

        Name = newName.Trim();
        UpdatedAtUtc = DateTime.UtcNow;
    }
    public void LinkToParentCategory(int parentCategoryId)
    {
        if (parentCategoryId < 0)
            throw new InvalidOperationException("Subcategory Parent cannot be below 0");

        if (parentCategoryId == Id)
            throw new InvalidOperationException("Subcategory Parent cannot be itself");

        ParentCategoryId = parentCategoryId;
        UpdatedAtUtc = DateTime.UtcNow;
    }
    public void Delete(int categoryId)
    {
        if (Deleted) return;

        Deleted = true;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
