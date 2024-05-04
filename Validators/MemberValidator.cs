using System.Text.RegularExpressions;
using FluentValidation;
using LibraryService.Models;

namespace LibraryService.Validators;

public class MemberValidator : AbstractValidator<Member>
{
    public MemberValidator()
    {
        RuleFor(p => p.FirstName)
            .NotEmpty().WithMessage("First Name is required.")
            .MaximumLength(50).WithMessage("First Name must not exceed 20 characters.");

        RuleFor(p => p.LastName)
            .NotEmpty().WithMessage("Last Name is required.")
            .MaximumLength(50).WithMessage("Last Name must not exceed 20 characters.");
        
        RuleFor(member => member.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress();

        RuleFor(member => member.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .NotNull()
            .MinimumLength(10)
            .MaximumLength(20)
            .Matches(new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"))
            .WithMessage("Phone number not valid");
    }
}