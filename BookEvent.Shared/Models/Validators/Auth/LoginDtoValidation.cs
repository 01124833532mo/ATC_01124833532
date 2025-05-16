using BookEvent.Shared.Models._Common;
using BookEvent.Shared.Models.Auth;
using FluentValidation;

namespace BookEvent.Shared.Models.Validators.Auth
{
    public class LoginDtoValidation : AbstractValidator<LoginDto>
    {
        public LoginDtoValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .Matches(RegexPatterns.Email)
                .WithMessage("Email is not valid");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .Matches(RegexPatterns.Password)
                .WithMessage("Password must be at least 8 characters long and contain at least one digit");

        }
    }
}
