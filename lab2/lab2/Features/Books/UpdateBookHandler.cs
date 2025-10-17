using lab2.Persistence;
using lab2.Validators;

namespace lab2.Features.Books;

public class UpdateBookHandler(BooksManagementContext dbContext)
{
    private readonly BooksManagementContext _dbContext = dbContext;
    
    public async Task<IResult> Handle(UpdateBookRequest request)
    {
        
        var validator = new CreateBookValidator();
        var validationResult = await validator.ValidateAsync(new CreateBookRequest(request.Title, request.Author, request.Year));
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }
        
        var book = await _dbContext.Books.FindAsync(request.Id);
        if (book == null)
        {
            return Results.NotFound();
        }

        book = new Book(request.Id, request.Title, request.Author, request.Year);
        return Results.Ok(book);
    }
}