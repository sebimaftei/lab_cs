using AutoMapper;
using ProductsManagement.Features.Products;
using ProductsManagement.Products.DTOs;

namespace ProductsManagement.Common.Resolvers;

public class BrandInitialsResolver : IValueResolver<Product, ProductProfileDTO, string>
{
    public string Resolve(Product source, ProductProfileDTO destination, string member, ResolutionContext context)
    {
        if (source.Brand == String.Empty) return "?";
        var inital1 = source.Brand[0];
        var inital2 = '\0';
        bool flag = false;
        
        for (int i = source.Brand.Length - 1; i >= 0; i--)
        {
            if (source.Brand[i] == ' ')
            {
                inital2 = source.Brand[i + 1];
                flag = true;
            }
        }

        return flag == false ? $"{inital1}" : $"{inital1}{inital2}";
    }   
}