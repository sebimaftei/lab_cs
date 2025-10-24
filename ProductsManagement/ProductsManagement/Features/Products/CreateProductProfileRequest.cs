namespace ProductsManagement.Products;

public class CreateProductProfileRequest
{
    public string Name { get; }
    public string Brand { get; }
    public string SKU { get; }
    public ProductCategory Category { get; }
    public decimal Price { get; }
    public DateTime ReleaseDate { get; }
    public string? ImageUrl { get; }
    public int StockQuantity { get; }
    
    public CreateProductProfileRequest(string name, string brand, string sku, ProductCategory category, decimal price, DateTime releaseDate, int stockQuantity)
    {
        this.Name = name;
        this.Brand = brand;
        this.SKU = sku;
        this.Category = category;
        this.Price = price;
        this.ReleaseDate = releaseDate;
        this.StockQuantity = stockQuantity;
    }
    
    public CreateProductProfileRequest(string name, string brand, string sku, ProductCategory category, decimal price, DateTime releaseDate)
    {
        this.Name = name;
        this.Brand = brand;
        this.SKU = sku;
        this.Category = category;
        this.Price = price;
        this.ReleaseDate = releaseDate;
    }
    
    public CreateProductProfileRequest(string name, string brand, string sku, ProductCategory category, decimal price, DateTime releaseDate, int stockQuantity, string imageUrl)
    {
        this.Name = name;
        this.Brand = brand;
        this.SKU = sku;
        this.Category = category;
        this.Price = price;
        this.ReleaseDate = releaseDate;
        this.StockQuantity = stockQuantity;
        this.ImageUrl = imageUrl;
    }
    
    public CreateProductProfileRequest(string name, string brand, string sku, ProductCategory category, decimal price, DateTime releaseDate, string imageUrl)
    {
        this.Name = name;
        this.Brand = brand;
        this.SKU = sku;
        this.Category = category;
        this.Price = price;
        this.ReleaseDate = releaseDate;
        this.ImageUrl = imageUrl;
    }
}