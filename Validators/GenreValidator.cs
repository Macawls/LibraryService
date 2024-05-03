using FluentValidation;
using LibraryService.Models;

namespace LibraryService.Validators;

public class GenreValidator : AbstractValidator<Genre>
{
    public GenreValidator()
    {
        RuleFor(genre => genre.Id)
            .NotEmpty();
        
        RuleFor(genre => genre.Name)
            .NotEmpty()
            .MaximumLength(15);
    }
}