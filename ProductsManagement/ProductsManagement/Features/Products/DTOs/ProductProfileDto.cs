namespace ProductsManagement.Products.DTOs;

public record ProductProfileDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Brand { get; init; }
    public string CategoryDisplayName { get; init; }
    public decimal Price { get; init; }
    public string FormattedPrice { get; init; }
    public DateTime ReleaseDate { get; init; }
    public DateTime CreatedAt { get; init; }
    public string? ImageUrl { get; init; }
    public bool IsAvailable { get; init; }
    public int StockQuantity { get; init; }
    public string ProductAge { get; init; }
    public string BrandInitials { get; init; }
    public string AvailabilityStatus { get; init; }
    public string Sku { get; init; }
}