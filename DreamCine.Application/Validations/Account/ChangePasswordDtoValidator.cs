using DreamCine.Application.DTOs.Account;
using FluentValidation;

namespace DreamCine.Application.Validations.Account
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password cannot be empty.");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password cannot be empty.")
                .MinimumLength(3).WithMessage("New password must be at least 3 characters.")
                .MaximumLength(30).WithMessage("New password must be at most 30 characters.");
        }
    }
}
