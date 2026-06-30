using DreamCine.Application.DTOs.Account;
using FluentValidation;

namespace DreamCine.Application.Validations.Account
{
    public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Please enter a valid email format.")
                .MaximumLength(100).WithMessage("Email must be at most 100 characters.");
        }
    }
}
