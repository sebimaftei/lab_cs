using AutoMapper;
using FluentValidation;
using ProductsManagement.Persistence;

namespace ProductsManagement.Features.Products;

public class UpdateProductHandler(ProductsProfileContext context, IMapper mapper, IValidator<UpdateProductRequest> validator)
{
    public async Task<IResult> Handle(UpdateProductRequest request)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        var updatedProduct = mapper.Map<Product>(request);
        
        context.Products.Update(updatedProduct);
        
        await context.SaveChangesAsync();
        return Results.NoContent();
    }
}