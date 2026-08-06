using Catalog.Application.Dtos;
using Catalog.Application.Usecases.Categories;

namespace Catalog.API.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api").WithTags("Category Management");
        group.MapGet("/categories", async (ICategoryService service) =>
        {
            IEnumerable<CategoryResponse> categories = await service.GetCategories();
            return categories is null || !categories.Any() ? Results.NotFound("No categories were found") : Results.Ok(categories);
        });
        group.MapGet("/category/{id}", async (ICategoryService service, int id) =>
        {
            CategoryResponse? category = await service.GetCategory(id);
            return category is null ? Results.NotFound("No categories were found") : Results.Ok(category);
        });
        group.MapPost("/category", async (ICategoryService service, CreateCategoryRequest request) =>
        {
            int categoryId = await service.CreateCategory(request);
            return Results.Created("/api/category/{id}", new { id = categoryId });
        });
        group.MapPut("/category/{id}", async (ICategoryService service, CreateCategoryRequest request, int id) =>
        {
            await service.UpdateCategoryDetails(request, id);
            return Results.NoContent();
        });
        group.MapDelete("/category/{id}", async (ICategoryService services, int id) =>
        {
            await services.RemoveCategory(id);
            return Results.NoContent();
        });
    }
}
