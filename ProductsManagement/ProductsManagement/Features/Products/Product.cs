using ProductsManagement.Products;

namespace ProductsManagement.Features.Products;

public record Product(Guid Id, 
    string Name, 
    string Brand, 
    string Sku, 
    ProductCategory Category, 
    decimal Price, DateTime ReleaseDate, 
    int StockQuantity = 0,
    string? ImageUrl = null
);