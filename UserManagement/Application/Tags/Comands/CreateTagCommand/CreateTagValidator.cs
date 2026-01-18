using FluentValidation;

namespace UserManagement.Application.Tags.Comands.CreateTagCommand
{
    public class CreateTagValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagValidator()
        {
            RuleFor(x => x.TagName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Tag name cannot be empty.")
                .Matches("^[a-zA-Z0-9]+$").WithMessage("Tag name cannot have any spaces or special characters.")
                .MaximumLength(20).WithMessage("Tag name cannot exceed 20 character.");

            RuleFor(x => x.TagDescription)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Tag description cannot be empty.")
                .MaximumLength(100).WithMessage("Tag description cannot exceed 100 character.");
        }
    }
}
