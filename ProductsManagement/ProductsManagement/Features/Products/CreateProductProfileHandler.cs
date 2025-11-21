using System.Diagnostics;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging.Abstractions;
using ProductsManagement.Common.Logging;
using ProductsManagement.Common.Mapping;
using ProductsManagement.Features.Products;
using ProductsManagement.Persistence;
using ProductsManagement.Products;
using ProductsManagement.Products.DTOs;

namespace ProductsManagement.Features.Products;

public class CreateProductProfileHandler(ProductsProfileContext context, ILogger<CreateProductProfileHandler> logger, IValidator<CreateProductProfileRequest> validator)
{
    
    private static IMapper CreateMapper()
    {
        var cfg = new MapperConfiguration(c => c.AddProfile(new AdvancedProductMappingProfile()),
            new NullLoggerFactory());
        return cfg.CreateMapper();
    }
    
    public async Task<IResult> Handle(CreateProductProfileRequest createProductProfileRequest)
    {
        Stopwatch stopWatchValidation = new Stopwatch();
        Stopwatch stopWatchTotal = new Stopwatch();
        stopWatchTotal.Start();
        stopWatchValidation.Start();
        TimeSpan validationTime=TimeSpan.FromSeconds(2);
        TimeSpan totalTime = System.TimeSpan.Zero;
        TimeSpan dbTime = System.TimeSpan.Zero;
        String? errorMessage = null;
        bool isValid = true;
        try
        {
            logger.LogInformation((int)LogEvents.ProductCreationStarted, $"Creating new product {createProductProfileRequest.Name} with the sku {createProductProfileRequest.Sku} and category {createProductProfileRequest.Category}");
            var validationResult = await validator.ValidateAsync(createProductProfileRequest);
            logger.LogInformation( (int)LogEvents.ProductValidationCompleted,"Validation has been performed");
            if (!validationResult.IsValid)
            {
                isValid = false;
                stopWatchValidation.Stop();
                stopWatchTotal.Stop();
                validationTime = stopWatchValidation.Elapsed;
                totalTime = stopWatchTotal.Elapsed;
                errorMessage = validationResult.Errors.Aggregate("", (current, error) => current + (error.ErrorMessage + ",\n"));
                String skuStatus = errorMessage.ToLower().Contains("sku") ? "failed": "success";
                String stockStatus = errorMessage.ToLower().Contains("stock") ? "failed": "success";
                logger.LogInformation((int)LogEvents.SKUValidationPerformed,"Sku Validation has been perfomed, it was a {status} ", skuStatus);
                logger.LogInformation((int)LogEvents.StockValidationPerformed," Stock Validation has been perfomed, it was a {status} ", stockStatus);
                throw new ValidationException(validationResult.Errors);
            }
            logger.LogInformation((int)LogEvents.SKUValidationPerformed,"Sku Validation has been perfomed, it was a success ");
            logger.LogInformation((int)LogEvents.StockValidationPerformed," Stock Validation has been perfomed, it was a success");
            stopWatchValidation.Stop();
            validationTime = stopWatchValidation.Elapsed;
            logger.LogInformation((int)LogEvents.DatabaseOperationStarted,$"Adding product {createProductProfileRequest.Name} with the sku {createProductProfileRequest.Sku} to the database ");
            Stopwatch stopWatchDB = new Stopwatch();
            stopWatchDB.Start();
            var mapper = CreateMapper();
            var product = mapper.Map<Product>(createProductProfileRequest);
            var result = mapper.Map<ProductProfileDTO>(product);
            
            context.Add(product);
            await context.SaveChangesAsync();
            
            stopWatchDB.Stop();
            stopWatchTotal.Stop();
            totalTime = stopWatchTotal.Elapsed;
            dbTime = stopWatchDB.Elapsed;
            logger.LogInformation((int)LogEvents.DatabaseOperationCompleted,$"{{code}} Database operation (Adding product {createProductProfileRequest.Name} with the sku {createProductProfileRequest.Sku}) successful!");
            return Results.Ok(result);
        }
        catch (Exception e)
        {
            if (isValid)
            {
                logger.LogError((int)LogEvents.DatabaseOperationFailed,"Database operation failed: {Message} ",e);
            }
            stopWatchTotal.Stop();
            errorMessage = e.Message;
            totalTime = stopWatchTotal.Elapsed;
            return Results.BadRequest(errorMessage);
        }
        finally
        {
            var metrics = new ProductCreationMetrics(Guid.NewGuid().ToString(), createProductProfileRequest.Name, createProductProfileRequest.Sku, createProductProfileRequest.Category, validationTime,
                    dbTime, totalTime,errorMessage==null, errorMessage);
            LoggingExtensions.LogProductCreationMetrics(logger, metrics);
        }
    }
}