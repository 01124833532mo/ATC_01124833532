using BookEvent.Shared.Models._Common;
using BookEvent.Shared.Models.Auth;
using FluentValidation;

namespace BookEvent.Shared.Models.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage("Current Password is required")
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("New Password is required")
                .Matches(RegexPatterns.Password)
                .WithMessage("New Password must contain at least one digit and be at least 8 characters long");

        }
    }
}
