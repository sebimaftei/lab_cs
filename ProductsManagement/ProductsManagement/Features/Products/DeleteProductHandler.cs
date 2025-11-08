using Microsoft.EntityFrameworkCore;
using ProductsManagement.Persistence;

namespace ProductsManagement.Features.Products;

public class DeleteProductHandler(ProductsProfileContext context)
{
    public async Task<IResult> Handle(DeleteProductRequest request)
    {
        var product = await context.Products.FirstOrDefaultAsync(u => u.Id == request.Id);
        if (product is null)
        {
            return Results.NotFound();
        }
        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }
}