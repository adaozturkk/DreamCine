using DreamCine.Application.DTOs.Genre;
using FluentValidation;

namespace DreamCine.Application.Validations.Genre
{
    public class CreateGenreDtoValidator : AbstractValidator<CreateGenreDto>
    {
        public CreateGenreDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name cannot be empty.")
                .MaximumLength(50).WithMessage("Name must be at most 50 characters.");
        }
    }
}
