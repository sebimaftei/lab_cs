namespace ProductsManagement.Products;

public record CreateProductProfileRequest(string Name, 
    string Brand, 
    string Sku, 
    ProductCategory Category, 
    decimal Price, 
    DateTime ReleaseDate, 
    int StockQuantity = 0,
    string? ImageUrl = null
);