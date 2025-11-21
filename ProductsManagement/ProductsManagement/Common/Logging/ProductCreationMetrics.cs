using ProductsManagement.Products;

namespace ProductsManagement.Common.Logging;

public record ProductCreationMetrics(
    string OperationId,
    string ProductName,
    string Sku,
    ProductCategory ProductCategory,
    TimeSpan ValidationDuration,
    TimeSpan DatabaseSaveDuration,
    TimeSpan TotalDuration,
    bool Success,
    string? ErrorReason);