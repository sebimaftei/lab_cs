using Microsoft.EntityFrameworkCore;
using ProductsManagement.Persistence;

namespace ProductsManagement.Features.Products;

public class GetAllProductsHandler(ProductsProfileContext context)
{
    public async Task<IResult> Handle(GetAllProductsRequest request)
    {
        var products = await context.Products.ToListAsync();
        return Results.Ok(products);
    }
}