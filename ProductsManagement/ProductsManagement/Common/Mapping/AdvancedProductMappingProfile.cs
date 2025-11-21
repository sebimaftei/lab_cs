using AutoMapper;
using ProductsManagement.Common.Resolvers;
using ProductsManagement.Features.Products;
using ProductsManagement.Products;
using ProductsManagement.Products.DTOs;

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
                src.StockQuantity > 0,
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
                src.StockQuantity > 0,
                src.StockQuantity, 
                src.ImageUrl));
        
        CreateMap<Product, ProductProfileDTO>().ForMember(dest => dest.CategoryDisplayName,
                opt => opt.MapFrom<CategoryDisplayResolver>())
            .ForMember(dest => dest.FormattedPrice,
                opt => opt.MapFrom<PriceFormatterResolver>())
            .ForMember(dest => dest.AvailabilityStatus,
                opt => opt.MapFrom<AvailabilityStatusResolver>())
            .ForMember(dest => dest.BrandInitials,
                opt => opt.MapFrom<BrandInitialsResolver>())
            .ForMember(dest => dest.ProductAge,
                opt => opt.MapFrom<ProductAgeResolver>())
            .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(src => src.Category != ProductCategory.Home ? src.ImageUrl : null))
            .ForMember(dest => dest.Price, opt
                => opt.MapFrom(src => src.Category != ProductCategory.Home ? src.Price : src.Price * (0.9m)))
            .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate))
            .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
            .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
            .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Sku));
    }
}