using lab2.Persistence;

namespace lab2.Features.Books;

public record UpdateBookRequest(int Id, string Title, string Author, int Year);