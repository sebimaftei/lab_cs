using FluentValidation;
using lab2.Features.Books;
using lab2.Persistence;
using lab2.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<BooksManagementContext>(options =>
    options.UseSqlite("Data Source=booksmanagement.db"));

builder.Services.AddScoped<CreateBooksHandler>();
builder.Services.AddScoped<GetAllBooksHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookValidator>();
builder.Services.AddScoped<DeleteBookHandler>();
builder.Services.AddScoped<GetBooksByIdHandler>();
builder.Services.AddScoped<GetBooksPageHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BooksManagementContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/books", async (CreateBookRequest req, CreateBooksHandler handler) =>
            await handler.Handle(req));

app.MapGet("/books", async (GetAllBooksHandler handler) =>
            await handler.Handle(new GetAllBooksRequest()));

app.MapDelete("/books/{id:int}", async (int id, DeleteBookHandler handler) =>
            await handler.Handle(new DeleteBooksRequest(id)));

app.MapGet("/books/{id:int}/}", async (int id, GetBooksByIdHandler handler) =>
    await handler.Handle(new GetBookByIdRequest(id)));

app.Run();