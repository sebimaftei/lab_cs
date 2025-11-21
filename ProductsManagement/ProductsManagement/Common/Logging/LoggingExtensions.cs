using ProductsManagement.Features.Products;

namespace ProductsManagement.Common.Logging;

public class LoggingExtensions
{
    public static void LogProductCreationMetrics(ILogger<CreateProductProfileHandler> logger,ProductCreationMetrics metrics)
    {
        logger.LogInformation(metrics.Success? (int)LogEvents.ProductCreationCompleted:(int)LogEvents.ProductValidationFailed,
            $"A product creation has been requested : \n" +
            "Product Information: {name}, {sku}, {category} \n" +
            "Validation time : {vtime}\n" +
            "Database operation time: {dbtime}\n" +
            "Total duration: {ttime} \n" +
            "Final Status: {succes} \n"+
            "Errors: {error}", 
            metrics.ProductName,metrics.Sku,metrics.ProductCategory,metrics.ValidationDuration.TotalMilliseconds,
            metrics.DatabaseSaveDuration != null ?  metrics.DatabaseSaveDuration.TotalMilliseconds : "na",
            metrics.TotalDuration,
            metrics.Success,
            metrics.ErrorReason!=null ? metrics.ErrorReason.ToString() : "na");
    }
}