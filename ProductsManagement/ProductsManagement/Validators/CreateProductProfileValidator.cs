using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductsManagement.Persistence;
using ProductsManagement.Products;

namespace ProductsManagement.Validators;

public class CreateProductProfileValidator : AbstractValidator<CreateProductProfileRequest>
{
    public CreateProductProfileValidator(ProductsProfileContext context)
    {
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