using FluentValidation;
using ProductsManagement.Features.Products;
using ProductsManagement.Persistence;
using ProductsManagement.Products;
using ProductsManagement.Products.DTOs;

namespace ProductsManagement.Features.Products;

public class CreateProductProfileHandler(ProductsProfileContext context, ILogger<CreateProductProfileHandler> logger, IValidator<CreateProductProfileRequest> validator)
{
    public async Task<IResult> Handle(CreateProductProfileRequest createProductProfileRequest)
    {
        logger.LogInformation("Creating product with Name: {Name}, and SKU: {Sku}", createProductProfileRequest.Name, createProductProfileRequest.Sku);
        
        // var validationResult = await validator.ValidateAsync(createProductProfileRequest);
        //
        // if (!validationResult.IsValid)
        // {
        //     throw new ValidationException(validationResult.Errors);
        // }
        
        var product = new Product(Guid.NewGuid(), createProductProfileRequest.Name, createProductProfileRequest.Brand, createProductProfileRequest.Sku, createProductProfileRequest.Category, createProductProfileRequest.Price, createProductProfileRequest.ReleaseDate, createProductProfileRequest.StockQuantity ,createProductProfileRequest.ImageUrl);
        context.Products.Add(product);
        await context.SaveChangesAsync();
        
        return Results.Created($"products/{product.Id}", product);
    }
}