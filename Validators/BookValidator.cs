using FluentValidation;
using LibraryService.Models;

namespace LibraryService.Validators;

public class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(book => book.PublicationYear)
            .NotEmpty()
            .GreaterThan(0)
            .InclusiveBetween(0, DateTime.Now.Year);
        
        RuleFor(book => book.ISBN)
            .NotEmpty().Length(10, 13);
        
        RuleFor(book => book.Author).NotEmpty()
            .MaximumLength(100);
        
        RuleFor(book => book.Title).NotEmpty()
            .MaximumLength(255);
    }
}