using BookEvent.Shared.Models._Common;
using BookEvent.Shared.Models.Auth;
using FluentValidation;

namespace BookEvent.Shared.Models.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Full name is required.")
                .MinimumLength(3)
                .WithMessage("Full name must be at least 3 characters long.");

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

        }
    }
}
