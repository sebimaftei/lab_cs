using lab2.Persistence;

namespace lab2.Features.Books;

public class DeleteBookHandler(BooksManagementContext dbContext)
{
    private readonly BooksManagementContext _dbContext = dbContext;

    public async Task<IResult> Handle(DeleteBooksRequest request)
    {
        var book = await _dbContext.Books.FindAsync(request.Id);
        if (book == null)
        {
            return Results.NotFound();
        }
        
        _dbContext.Books.Remove(book);
        await _dbContext.SaveChangesAsync();
        return Results.NoContent();
    }
}