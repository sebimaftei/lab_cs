using lab2.Persistence;

namespace lab2.Features.Books;

public class GetBooksByIdHandler(BooksManagementContext dbContext)
{
    private readonly BooksManagementContext _dbContext = dbContext;
    
    public async Task<IResult> Handle(GetBookByIdRequest request)
    {
        var book = await _dbContext.Books.FindAsync(request.Id);
        if (book == null)
        {
            return Results.NotFound();
        }
        
        return Results.Ok(book);
    }
}