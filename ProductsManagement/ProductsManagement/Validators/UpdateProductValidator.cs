using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductsManagement.Features.Products;
using ProductsManagement.Persistence;

namespace ProductsManagement.Validators;

public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductValidator(ProductsProfileContext context)
    {
        RuleFor(x => x.Id)
            .NotNull().NotEmpty()
            .NotEqual(Guid.Empty).WithMessage("Id must be a valid GUID.")
            .MustAsync(async (id, cancellation) =>
            {
                if (id == Guid.Empty) return false;
                return await context.Products.AnyAsync(u => u.Id == id, cancellation);
            })
            .WithMessage("User with the specified Id does not exist.");
        
        RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(50).MinimumLength(4);
        RuleFor(x => x.Brand).NotNull().NotEmpty().MaximumLength(50);
        RuleFor(x => x.Sku).NotNull().NotEmpty()
            .MustAsync(async (sku, cancellation) =>
            {
                if (sku is null) return false;
                return await context.Products.AnyAsync(u => u.Sku == sku);
            })
            .WithMessage("SKU must be unique.");
    }
}