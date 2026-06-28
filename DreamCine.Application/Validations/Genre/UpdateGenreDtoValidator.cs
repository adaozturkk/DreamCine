using DreamCine.Application.DTOs.Genre;
using FluentValidation;

namespace DreamCine.Application.Validations.Genre
{
    public class UpdateGenreDtoValidator : AbstractValidator<UpdateGenreDto>
    {
        public UpdateGenreDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name cannot be empty.")
                .MaximumLength(50).WithMessage("Name must be at most 50 characters.");
        }
    }
}
