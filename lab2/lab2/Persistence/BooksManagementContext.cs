using lab2.Features.Books;
using Microsoft.EntityFrameworkCore;

namespace lab2.Persistence;

public class BooksManagementContext(DbContextOptions<BooksManagementContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
}