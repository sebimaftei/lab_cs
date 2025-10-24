namespace ProductsManagement.Products;

public class Product
{
    string Name { get; set; }
    string Brand { get; set; }
    string SKU { get; set; }
    ProductCategory Category { get; set; }
    decimal Price { get; set; }
    DateTime ReleaseDate { get; set; }
    string? ImageUrl { get; set; }
    bool IsAvailable { get; set; }
    int StockQuantity { get; set; } = 0;

    public Product(string name, string brand, string sku, ProductCategory category, decimal price, DateTime releaseDate, int stockQuantity)
    {
        this.Name = name;
        this.Brand = brand;
        this.SKU = sku;
        this.Category = category;
        this.Price = price;
        this.ReleaseDate = releaseDate;
        this.StockQuantity = stockQuantity;
        this.IsAvailable = StockQuantity > 0;
    }
    
    public Product(string name, string brand, string sku, ProductCategory category, decimal price, DateTime releaseDate, int stockQuantity, string imageUrl)
    {
        this.Name = name;
        this.Brand = brand;
        this.SKU = sku;
        this.Category = category;
        this.Price = price;
        this.ReleaseDate = releaseDate;
        this.StockQuantity = stockQuantity;
        this.IsAvailable = StockQuantity > 0;
        this.ImageUrl = imageUrl;
    }
    
    public Product(string name, string brand, string sku, ProductCategory category, decimal price, DateTime releaseDate)
    {
        this.Name = name;
        this.Brand = brand;
        this.SKU = sku;
        this.Category = category;
        this.Price = price;
        this.ReleaseDate = releaseDate;
        this.IsAvailable = StockQuantity > 0;
    }
    
    public Product(string name, string brand, string sku, ProductCategory category, decimal price, DateTime releaseDate, string imageUrl)
    {
        this.Name = name;
        this.Brand = brand;
        this.SKU = sku;
        this.Category = category;
        this.Price = price;
        this.ReleaseDate = releaseDate;
        this.IsAvailable = StockQuantity > 0;
        this.ImageUrl = imageUrl;
    }
}