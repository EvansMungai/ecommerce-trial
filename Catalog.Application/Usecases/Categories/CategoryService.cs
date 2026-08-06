using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;

namespace Catalog.Application.Usecases.Categories;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _repository;
    private readonly IUnitOfWork _unitOfWork;
    public CategoryService(IRepository<Category> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> CreateCategory(CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        Category category = new Category(request.Name, request.ParentCategoryId);
        _repository.AddAsync(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return category.Id;
    }

    public async Task<IEnumerable<CategoryResponse>> GetCategories(CancellationToken cancellationToken = default)
    {
        IEnumerable<Category> categories = await _repository.GetAllAsync(cancellationToken);
        return categories.Where(c => !c.Deleted)
            .Select(c => new CategoryResponse(c.Name, c.ParentCategoryId, c.CreatedAtUtc, c.UpdatedAtUtc)).ToList();
    }

    public async Task<CategoryResponse?> GetCategory(int id, CancellationToken cancellationToken)
    {
        Category? category = await _repository.GetSingleAsync(id, cancellationToken);
        if (category is null || category.Deleted)
            return null;

        return new CategoryResponse(category.Name, category.ParentCategoryId, category.CreatedAtUtc, category.UpdatedAtUtc);
    }

    public async Task RemoveCategory(int categoryId, CancellationToken cancellationToken = default)
    {
        Category? category = await _repository.GetSingleAsync(categoryId, cancellationToken);
        if (category is null)
            throw new KeyNotFoundException("Category not found");

        category.Delete(categoryId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateCategoryDetails(CreateCategoryRequest request, int id, CancellationToken cancellationToken = default)
    {
        Category? category = await _repository.GetSingleAsync(id, cancellationToken);
        if (category is null)
            throw new KeyNotFoundException("Category not found");

        category.Rename(request.Name);
        if (request.ParentCategoryId.HasValue)
            category.LinkToParentCategory(request.ParentCategoryId.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
