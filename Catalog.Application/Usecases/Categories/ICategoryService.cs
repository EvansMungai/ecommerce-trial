using Catalog.Application.Dtos;

namespace Catalog.Application.Usecases.Categories;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponse>> GetCategories(CancellationToken cancellationToken = default);
    Task<CategoryResponse?> GetCategory(int id, CancellationToken cancellationToken = default);
    Task<int> CreateCategory(CreateCategoryRequest request, CancellationToken cancellationToken = default);
    Task UpdateCategoryDetails(CreateCategoryRequest request, int id, CancellationToken cancellationToken = default);
    Task RemoveCategory(int categoryId, CancellationToken cancellationToken = default);
}
