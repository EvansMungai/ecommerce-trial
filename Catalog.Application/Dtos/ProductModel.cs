namespace Catalog.Application.Dtos;

public record CreateProductRequest(string Name, string Description, string Sku, int MinStockQuantity, int LowStockActivityId);

public record ProductResponse(string Name, string Description, string sku, int MinStockQuantity, int StockQuantity);

public record RestockProductRequest(int StockQuantity);

public record SetProductStockRulesRequest(int MinStockQuantity, int LowStockActivityId);

public record SetProductOrderRulesRequest(int MinOrderQuantity, int MaxOrderQuantity);

public record AssignCategoryToProductRequest(int CategoryId);