using DreamCine.Application.DTOs.Account;
using FluentValidation;

namespace DreamCine.Application.Validations.Account
{
    public class AssignRoleDtoValidator : AbstractValidator<AssignRoleDto>
    {
        public AssignRoleDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Please enter a valid email format.")
                .MaximumLength(100).WithMessage("Email must be at most 100 characters.");
            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role cannot be empty.")
                .IsInEnum().WithMessage("Role must be 'Admin', 'User', or 'Staff'.");
        }
    }
}
