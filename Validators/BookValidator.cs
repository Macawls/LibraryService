using FluentValidation;
using LibraryService.Models;

namespace LibraryService.Validators;

public class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(book => book.PublicationYear)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Publication year must be greater than 0")
            .InclusiveBetween(0, DateTime.Now.Year);
        
        RuleFor(book => book.ISBN)
            .NotEmpty().WithMessage("ISBN number is required")
            .Length(10, 13);
        
        RuleFor(book => book.Author)
            .NotEmpty().WithMessage("Author is required")
            .MaximumLength(100);
        
        RuleFor(book => book.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(255);
    }
}