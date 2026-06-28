using DreamCine.Application.DTOs.Movie;
using FluentValidation;

namespace DreamCine.Application.Validations.Movie
{
    public class UpdateMovieDtoValidator : AbstractValidator<UpdateMovieDto>
    {
        public UpdateMovieDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title cannot be empty.")
                .MaximumLength(200).WithMessage("Title cannot be longer than 200 characters.");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description cannot be empty.")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters.")
                .MaximumLength(500).WithMessage("Description must be at most 500 characters");
            RuleFor(x => x.Duration)
                .NotEmpty().WithMessage("Duration cannot be empty.")
                .InclusiveBetween(5, 400).WithMessage("Duration must be between 5 and 400 minutes.");
            RuleFor(x => x.Rating)
                .InclusiveBetween(0, 10).WithMessage("Rating must be between 0 and 10 stars.");
            RuleFor(x => x.ReleaseDate)
                .NotEmpty().WithMessage("Release date cannot be empty.")
                .GreaterThan(new DateTime(1880, 1, 1)).WithMessage("Release date must be after 1880.")
                .LessThan(DateTime.Now.AddYears(10)).WithMessage("Release date cannot be more than 10 years in the future.");
            RuleFor(x => x.GenreIds)
                .NotEmpty().WithMessage("A movie must have at least one genre.");
            RuleFor(x => x.StatusId)
                .GreaterThan(0).WithMessage("Status must be selected.");
        }
    }
}
