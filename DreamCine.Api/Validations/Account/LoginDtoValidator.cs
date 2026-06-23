using DreamCine.Api.DTOs.Account;
using FluentValidation;

namespace DreamCine.Api.Validations.Account
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Please enter a valid email format.")
                .MaximumLength(100).WithMessage("Email must be at most 100 characters.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .MinimumLength(3).WithMessage("Password must be at least 3 characters.")
                .MaximumLength(30).WithMessage("Password must be at most 30 characters.");
        }
    }
}
