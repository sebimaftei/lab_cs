using lab2.Persistence;
using Microsoft.EntityFrameworkCore;

namespace lab2.Features.Books;

public class GetBooksPageHandler(BooksManagementContext context)
{
    private readonly BooksManagementContext _context = context;

    public async Task<IResult> Handle(GetBooksPageRequest request)
    {
        var query = _context.Books.AsQueryable();

        if (request.AuthorFiltering is not null)
        {
            query = query.Where(b => b.Author == request.AuthorFiltering);
        }

        if (request.SortBy is not null)
        {
            if (request.SortBy == "Year") { query = query.OrderBy(b => b.Year); }
            else if (request.SortBy == "Author") { query = query.OrderBy(b => b.Title); }
        }
        
        var books = await query.Skip(request.NrPage * request.NrPerPage).Take(request.NrPerPage).ToListAsync();
        return Results.Ok(books);
    }
    
}