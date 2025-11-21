using AutoMapper;
using ProductsManagement.Features.Products;
using ProductsManagement.Products.DTOs;

namespace ProductsManagement.Common.Resolvers;

public class PriceFormatterResolver : IValueResolver<Product, ProductProfileDTO, string>
{
    public string Resolve(Product source, ProductProfileDTO destination, string member, ResolutionContext context)
    {
        return source.Price.ToString("C2");
    }   
}