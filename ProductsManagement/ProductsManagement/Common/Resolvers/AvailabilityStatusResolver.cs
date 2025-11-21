using AutoMapper;
using ProductsManagement.Features.Products;
using ProductsManagement.Products.DTOs;

namespace ProductsManagement.Common.Resolvers;

public class AvailabilityStatusResolver : IValueResolver<Product, ProductProfileDTO, string>
{
    public string Resolve(Product source, ProductProfileDTO destination, string member, ResolutionContext context)
    {
        if (!source.IsAvailable)
        {
            return "Out of stock";
        }

        switch (source.StockQuantity)
        {
            case 0: return "Unavailable";
            case 1: return "Last Item";
            case <= 5: return "Limited Stock";
            default:
                return "In Stock";
        }
    }   
}