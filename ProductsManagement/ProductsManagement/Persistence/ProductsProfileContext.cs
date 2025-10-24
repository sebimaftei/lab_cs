using Microsoft.EntityFrameworkCore;

namespace ProductsManagement.Persistence;

public class ProductsProfileContext(DbContextOptions<ProductsProfileContext> options) : DbContext(options)
{
    
}