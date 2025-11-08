using AutoMapper;
using ProductsManagement.Features.Products;
using ProductsManagement.Products;

namespace ProductsManagement.Common.Mapping;

public class AdvancedProductMappingProfile : Profile
{
    public AdvancedProductMappingProfile()
    {
        CreateMap<Product, CreateProductProfileRequest>();

        CreateMap<CreateProductProfileRequest, Product>()
            .ConstructUsing(src => new Product(Guid.NewGuid(),
                src.Name, 
                src.Brand,
                src.Sku, 
                src.Category, 
                src.Price, 
                src.ReleaseDate, 
                src.StockQuantity, 
                src.ImageUrl));
        
        CreateMap<UpdateProductRequest, Product>()
            .ConstructUsing(src => new Product(src.Id,
                src.Name, 
                src.Brand,
                src.Sku, 
                src.Category, 
                src.Price, 
                src.ReleaseDate, 
                src.StockQuantity, 
                src.ImageUrl));
    }
}