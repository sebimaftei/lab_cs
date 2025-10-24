namespace ProductsManagement.Products.DTOs;

public class ProductProfileDto
{
    Guid Id;
    string Name;
    string Brand;
    string SKU;
    string CategoryDisplayName;
    decimal Price;
    string FormattedPrice;
    DateTime ReleaseDate;
    private DateTime CreatedAt;
    string? ImageUrl;
    bool IsAvailable;
    int StockQuantity;
    string ProductAge;
    string BrandInitials;
    string AvailabilityStatus;

    public ProductProfileDto(Guid id, string name, string brand, string sku, string categoryDisplayName,
        string productAge, decimal price, DateTime releaseDate, string imageUrl, int stockQuantity, DateTime createdAt)
    {
        this.Id = id;
        this.Name = name;
        this.Brand = brand;
        this.CategoryDisplayName = categoryDisplayName;
        this.ProductAge = productAge;
        this.SKU = sku;
        this.Price = price;
        this.ReleaseDate = releaseDate;
        this.ImageUrl = imageUrl;
        this.StockQuantity = stockQuantity;
        this.IsAvailable = this.StockQuantity > 0;
        this.FormattedPrice = this.Price.ToString("C");
        this.CreatedAt = createdAt;
        this.AvailabilityStatus = IsAvailable ? "Available" : "Unavailable";
    }
    
    public ProductProfileDto(Guid id, string name, string brand, string sku, string categoryDisplayName,
        string productAge, decimal price, DateTime releaseDate, int stockQuantity, DateTime createdAt)
    {
        this.Id = id;
        this.Name = name;
        this.Brand = brand;
        this.CategoryDisplayName = categoryDisplayName;
        this.ProductAge = productAge;
        this.SKU = sku;
        this.Price = price;
        this.ReleaseDate = releaseDate;
        this.StockQuantity = stockQuantity;
        this.IsAvailable = this.StockQuantity > 0;
        this.FormattedPrice = this.Price.ToString("C");
        this.CreatedAt = createdAt;
        this.AvailabilityStatus = IsAvailable ? "Available" : "Unavailable";
    }
}