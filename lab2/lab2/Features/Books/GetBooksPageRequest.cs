namespace lab2.Features.Books;

public record GetBooksPageRequest(int NrPage, int NrPerPage, string? AuthorFiltering, string? SortBy);