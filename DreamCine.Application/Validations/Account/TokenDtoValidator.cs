using DreamCine.Application.DTOs.Account;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamCine.Application.Validations.Account
{
    public class TokenDtoValidator : AbstractValidator<TokenDto>
    {
        public TokenDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Please enter a valid email format.")
                .MaximumLength(100).WithMessage("Email must be at most 100 characters.");
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token cannot be empty.");
        }
    }
}
