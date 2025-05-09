using BookEvent.Shared.Models.Categories;
using FluentValidation;

namespace BookEvent.Shared.Models.Validators.Categories
{
    public class CategoryDtoValidator : AbstractValidator<CategoryDto>
    {
        public CategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MinimumLength(3)
                .WithMessage("Name must be at least 3 characters long");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required")
                .MinimumLength(10)
                .WithMessage("Description must be at least 10 characters long");

        }
    }
}
