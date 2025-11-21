using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductsManagement.Common.Mapping;
using ProductsManagement.Features.Products;
using ProductsManagement.Persistence;
using ProductsManagement.Products;
using ProductsManagement.Validators;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Product Management API",
            Version = "v1",
            Description = "Product Management API",
            Contact = new OpenApiContact
            {
                Name = "API Support",
                Email = "api-support@gmail.com",
            }
        });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ProductsProfileContext>(options =>
    options.UseSqlite("Data Source=products.db"));
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AdvancedProductMappingProfile>(),
    typeof(AdvancedProductMappingProfile));

builder.Services.AddScoped<CreateProductProfileHandler>();
builder.Services.AddScoped<GetAllProductsHandler>();
builder.Services.AddScoped<GetProductByIdHandler>();
builder.Services.AddScoped<DeleteProductHandler>();
builder.Services.AddScoped<UpdateProductHandler>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateProductProfileValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProductValidator>();
//builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCorrelationIdMiddleware();

// Ensure the database is created at runtime
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ProductsProfileContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI
    (
        c=>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products Management API V1");
            c.RoutePrefix = "swagger"; // Set Swagger UI at app's root
            c.DisplayRequestDuration();
        }
    );
    
    app.MapOpenApi();
}

// Enable CORS early in the pipeline
app.UseCors("DevCors");

app.UseHttpsRedirection();

app.MapPost("/products", async ([FromBody]CreateProductProfileRequest req, CreateProductProfileHandler handler) =>
    await handler.Handle(req));
app.MapGet("/products", async (GetAllProductsHandler handler) =>
    await handler.Handle(new GetAllProductsRequest()));
app.MapDelete("/products/{id:guid}", async (Guid id, DeleteProductHandler handler) =>
    await handler.Handle(new DeleteProductRequest(id)));
app.MapGet("/products/{id:guid}", async (Guid id, GetProductByIdHandler handler) =>
    await handler.Handle(new GetProductByIdRequest(id)));
app.MapPut("/products/{id:guid}",
    async (Guid id, [FromBody]UpdateProductRequest req, UpdateProductHandler handler) =>
    {
        var updateRequest = req with { Id = id };
        var result = await handler.Handle(updateRequest);
        return result;
    });
app.Run();