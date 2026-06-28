using DreamCine.Application.DTOs.Status;
using FluentValidation;

namespace DreamCine.Application.Validations.Status
{
    public class UpdateStatusDtoValidator : AbstractValidator<UpdateStatusDto>
    {
        public UpdateStatusDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Status name cannot be empty.")
                .MaximumLength(20).WithMessage("Status name cannot be longer than 20 characters.");
        }
    }
}
