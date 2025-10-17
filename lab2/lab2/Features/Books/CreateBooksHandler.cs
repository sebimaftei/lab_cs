using lab2.Persistence;
using lab2.Validators;

namespace lab2.Features.Books;

public class CreateBooksHandler(BooksManagementContext context)
{
    private readonly BooksManagementContext _context = context;
    static int _nextId = 0;

    public async Task<IResult> Handle(CreateBookRequest request)
    {
        var validator = new CreateBookValidator();
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }
        var book = new Book(_nextId, request.Title, request.Author, request.Year);
        _nextId++;
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        
        return Results.Created($"/books/{book.Id}", book);
    }
}