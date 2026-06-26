using DreamCine.Api.DTOs.MovieSession;
using FluentValidation;

namespace DreamCine.Api.Validations.MovieSession
{
    public class UpdateMovieSessionDtoValidator : AbstractValidator<UpdateMovieSessionDto>
    {
        public UpdateMovieSessionDtoValidator()
        {
            RuleFor(x => x.MovieId)
                .GreaterThan(0).WithMessage("Movie id must be greater than 0.");
            RuleFor(x => x.ScreenId)
                .GreaterThan(0).WithMessage("Screen id must be greater than 0.");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(x => x.SessionTime)
                .GreaterThan(DateTime.Now).WithMessage("Session time cannot be in past.");
        }
    }
}
