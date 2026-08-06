namespace Catalog.Application.Dtos;

public record CreateCategoryRequest(string Name, int? ParentCategoryId);

public record CategoryResponse(string Name, int? ParentCategoryId, DateTime CreatedAt, DateTime UpdatedAt);
