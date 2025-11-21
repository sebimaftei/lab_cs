using AutoMapper;
using ProductsManagement.Features.Products;
using ProductsManagement.Products.DTOs;

namespace ProductsManagement.Common.Resolvers;

public class ProductAgeResolver : IValueResolver<Product, ProductProfileDTO, string>
{
    public string Resolve(Product source, ProductProfileDTO destination, string member, ResolutionContext context)
    {
        DateTime currentDate = DateTime.Now;
        TimeSpan age = currentDate - source.ReleaseDate;

        switch (age.Days)
        {
            case < 30:
                return "New Release";
            case < 365:
            {
                var monthsOld = age.Days / 30;
                return$"{monthsOld} months old";
            }
            case < 1825:
            {
                int yearsOld = age.Days / 365;
                return$"{yearsOld} years old";
            }
            default:
                return "Classic";
        }
    }   
}