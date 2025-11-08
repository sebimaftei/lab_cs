using ProductsManagement.Persistence;

namespace ProductsManagement.Features.Products;

public class GetProductByIdHandler(ProductsProfileContext context)
{
        public async Task<IResult> Handle(GetProductByIdRequest request)
        {
                var product = await context.Products.FindAsync(request.Id);
                return product is null ? Results.NotFound() : Results.Ok(product);
        }
}