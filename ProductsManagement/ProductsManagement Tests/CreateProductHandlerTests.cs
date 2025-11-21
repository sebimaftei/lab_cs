using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsManagement.Common.Mapping;
using ProductsManagement.Features.Products;
using ProductsManagement.Persistence;
using ProductsManagement.Products;
using ProductsManagement.Products.DTOs;
using ProductsManagement.Validators;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ProductsManagement_Tests;

public class CreateProductHandlerTests : IDisposable
{
    private ProductsProfileContext _context;
    private readonly Mock<IValidator<CreateProductProfileRequest>> _validatorMock;
    private readonly Mock<ILogger<CreateProductProfileHandler>> _loggerMock;

    // Constructor to set up the mock logger and context
    public CreateProductHandlerTests()
    {
        _context = CreateInMemoryDbContext();
        _loggerMock = new Mock<ILogger<CreateProductProfileHandler>>();
    }

    // Creates an in-memory database context
    private ProductsProfileContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ProductsProfileContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ProductsProfileContext(options);
    }

    [Fact]
    public async Task Handle_ValidElectronicsProductRequest_CreatesProductWithCorrectMappings()
    {
        // Arrange: Create a valid Electronics product request
        var createProductProfileRequest = new CreateProductProfileRequest(
            Name: "Awesome app",
            Brand: "Super Tech",
            Sku: "12345",
            Category: ProductCategory.Electronics,
            Price: 1000m,
            ReleaseDate: new DateTime(2022, 5, 1),
            StockQuantity: 5,
            ImageUrl: "https://example.com/image.jpg"
        );

        // Act: Call the handler with a mocked validator
        var validator = new CreateProductProfileValidator(_context);
        var handler = new CreateProductProfileHandler(_context, _loggerMock.Object, validator);
        var result = await handler.Handle(createProductProfileRequest);

        // Assert: Verify Created result type
        result.Should().BeOfType<Ok<ProductProfileDTO>>();
        var createdProduct = ((Ok<ProductProfileDTO>)result).Value;

        // Assert: Check CategoryDisplayName = "Electronics & Technology"
        createdProduct.CategoryDisplayName.Should().Be("Electronics & Technology");

        // Assert: Check BrandInitials for two-word brand
        createdProduct.BrandInitials.Should().Be("ST");

        // Assert: Check ProductAge calculation
        var expectedAge = DateTime.Now.Year - createProductProfileRequest.ReleaseDate.Year;
        createdProduct.ProductAge.Should().Be(expectedAge.ToString() + " years old");

        // Assert: Check FormattedPrice starts with currency symbol
        createdProduct.FormattedPrice.Should().StartWith("\u00a3");

        // Assert: Check AvailabilityStatus based on stock
        createdProduct.AvailabilityStatus.Should().Be("Limited Stock");

        // Assert: Check other relevant properties of ProductProfileDTO
        createdProduct.Name.Should().Be(createProductProfileRequest.Name);
        createdProduct.Brand.Should().Be(createProductProfileRequest.Brand);
        createdProduct.Sku.Should().Be(createProductProfileRequest.Sku);
        createdProduct.Price.Should().Be(createProductProfileRequest.Price);
        createdProduct.ReleaseDate.Should().Be(createProductProfileRequest.ReleaseDate);
        createdProduct.ImageUrl.Should().Be(createProductProfileRequest.ImageUrl);
        createdProduct.StockQuantity.Should().Be(createProductProfileRequest.StockQuantity);

    }

    [Fact]
    public async Task Handle_DuplicateSKU_ThrowsValidationExceptionWithLogging()
    {
        // Arrange: Create a product with duplicate SKU
        var createProductProfileRequest = new CreateProductProfileRequest(
            Name: "Duplicate SKU Product",
            Brand: "Tech Brand",
            Sku: "DUPLICATE_SKU",
            Category: ProductCategory.Electronics,
            Price: 1000m,
            ReleaseDate: new DateTime(2022, 5, 1),
            StockQuantity: 5,
            ImageUrl: "https://example.com/image.jpg"
        );

        // Insert a product with the same SKU in the in-memory database
        _context.Products.Add(new Product
        (
            Guid.NewGuid(),
            Name: "Old Product",
            Brand: "Tech Brand",
            Sku: "DUPLICATE_SKU",
            Category: ProductCategory.Electronics,
            Price: 500m,
            ReleaseDate: new DateTime(2020, 1, 1),
            true,
            StockQuantity: 10,
            ImageUrl: "https://example.com/old-product.jpg"
        ));
        await _context.SaveChangesAsync();

        // Act: Call the handler
        var validator = new CreateProductProfileValidator(_context);
        var handler = new CreateProductProfileHandler(_context, _loggerMock.Object, validator);
        var result = await handler.Handle(createProductProfileRequest);

        // Assert: The result should be a BadRequest with validation error
        result.Should().BeOfType<BadRequest<string>>();
    }

    [Fact]
    public async Task Handle_HomeProductRequest_AppliesDiscountAndConditionalMapping()
    {
        // Arrange: Create a Home product request
        var createProductProfileRequest = new CreateProductProfileRequest(
            Name: "Home Product",
            Brand: "Home Brand",
            Sku: "HOME123",
            Category: ProductCategory.Home,
            Price: 200m,
            ReleaseDate: new DateTime(2022, 5, 1),
            StockQuantity: 20,
            ImageUrl: "https://example.com/home-product.jpg"
        );

        // Act: Call the handler
        var validator = new CreateProductProfileValidator(_context);
        var handler = new CreateProductProfileHandler(_context, _loggerMock.Object, validator);
        var result = await handler.Handle(createProductProfileRequest);

        // Assert: The result should be an Ok response
        result.Should().BeOfType<Ok<ProductProfileDTO>>();
        var createdProduct = ((Ok<ProductProfileDTO>)result).Value;

        // Assert: Check if the price has been discounted for a Home product
        createdProduct.Price.Should().Be(180); // 10% discount applied

        // Assert: Check if the ImageUrl was set to null for Home category
        createdProduct.ImageUrl.Should().BeNull();
    }

    public void Dispose()
    {
        // Cleanup resources if needed, e.g., resetting in-memory context
        _context.Dispose();
    }
}
