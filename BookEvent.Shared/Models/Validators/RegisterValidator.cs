using BookEvent.Shared.Models._Common;
using BookEvent.Shared.Models.Auth;
using FluentValidation;

namespace BookEvent.Shared.Models.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Full Name is required")
                .MinimumLength(3)
                .WithMessage("Full Name must be at least 3 characters long");
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .Matches(RegexPatterns.Email)
                .WithMessage("Email is not valid");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone Number is required")
                .Matches(RegexPatterns.PhoneNumber)
                .WithMessage("Phone Number is not valid");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .Matches(RegexPatterns.Password)
                .WithMessage("Password must contain at least one digit and be at least 8 characters long");

        }
    }
}
