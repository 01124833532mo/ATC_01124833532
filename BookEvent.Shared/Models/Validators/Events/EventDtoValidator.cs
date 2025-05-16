using BookEvent.Shared.Models.Events;
using FluentValidation;

namespace BookEvent.Shared.Models.Validators.Events
{
    public class EventDtoValidator : AbstractValidator<EventDto>
    {
        public EventDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters");
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required")
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters");
            RuleFor(x => x.Venue)
                .NotEmpty()
                .WithMessage("Venue is required")
                .MaximumLength(200)
                .WithMessage("Venue must not exceed 200 characters");
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Price must be a positive number");

            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Date is required")
                .Must(date => date > DateTime.Now)
                .WithMessage("Date must be in the future");

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("CategoryId is required")
                .GreaterThan(0)
                .WithMessage("CategoryId must be a positive number");


        }
    }

}
