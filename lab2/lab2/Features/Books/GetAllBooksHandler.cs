using lab2.Persistence;
using Microsoft.EntityFrameworkCore;

namespace lab2.Features.Books;

public class GetAllBooksHandler(BooksManagementContext context)
{
    private readonly BooksManagementContext _context = context;

    public async Task<IResult> Handle(GetAllBooksRequest req)
    {
        var users = await _context.Books.ToListAsync();
        return Results.Ok(users);
    }
}
