using AutoMapper;
using ProductsManagement.Features.Products;
using ProductsManagement.Products;
using ProductsManagement.Products.DTOs;

namespace ProductsManagement.Common.Resolvers;

public class CategoryDisplayResolver : IValueResolver<Product, ProductProfileDTO, string>
{
    public string Resolve(Product source, ProductProfileDTO destination, string member, ResolutionContext context)
    {
        switch (source.Category)
        {
            case ProductCategory.Books:
                return "Books & Media";
            case ProductCategory.Clothing:
                return "Clothing & Fashion";
            case ProductCategory.Electronics:
                return "Electronics & Technology";
            case ProductCategory.Home:
                return "Home & Garden";
            default:
                return "Uncategorized";
        }
    }   
}