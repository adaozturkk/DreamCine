using DreamCine.Api.DTOs.Genre;
using FluentValidation;

namespace DreamCine.Api.Validations.Genre
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
