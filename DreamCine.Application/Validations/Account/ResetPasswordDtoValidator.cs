using DreamCine.Application.DTOs.Account;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamCine.Application.Validations.Account
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Please enter a valid email format.")
                .MaximumLength(100).WithMessage("Email must be at most 100 characters.");
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token cannot be empty.");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password cannot be empty.")
                .MinimumLength(3).WithMessage("New password must be at least 3 characters.")
                .MaximumLength(30).WithMessage("New password must be at most 30 characters.");
        }
    }
}
