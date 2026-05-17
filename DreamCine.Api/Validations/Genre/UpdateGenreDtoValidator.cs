using DreamCine.Api.DTOs.Genre;
using FluentValidation;

namespace DreamCine.Api.Validations.Genre
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
