using Microsoft.EntityFrameworkCore;
using ProductsManagement.Features.Products;

namespace ProductsManagement.Persistence;

public class ProductsProfileContext(DbContextOptions<ProductsProfileContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
}