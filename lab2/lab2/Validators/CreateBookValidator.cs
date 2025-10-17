using FluentValidation;
using lab2.Features.Books;

namespace lab2.Validators;

public class CreateBookValidator : AbstractValidator<CreateBookRequest>
{
    public CreateBookValidator()
    {
        RuleFor(x => x.Title).NotEmpty().NotNull().WithName("Title must not be null or empty");
        RuleFor(x => x.Author).NotEmpty().NotNull().WithName("Author must not be null or empty");
    }
}