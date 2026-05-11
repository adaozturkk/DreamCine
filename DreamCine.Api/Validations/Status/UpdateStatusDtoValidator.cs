using DreamCine.Api.DTOs.Status;
using FluentValidation;

namespace DreamCine.Api.Validations.Status
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
