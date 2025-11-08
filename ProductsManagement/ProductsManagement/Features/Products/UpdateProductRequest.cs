using ProductsManagement.Products;

namespace ProductsManagement.Features.Products;

public record UpdateProductRequest(Guid Id, string Name, string Brand, string Sku, ProductCategory Category, decimal Price, DateTime ReleaseDate, int StockQuantity, string ImageUrl);