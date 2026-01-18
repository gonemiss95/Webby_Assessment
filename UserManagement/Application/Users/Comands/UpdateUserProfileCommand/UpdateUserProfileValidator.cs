using FluentValidation;

namespace UserManagement.Application.Users.Comands.UpdateUserProfileCommand
{
    public class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileCommand>
    {
        public UpdateUserProfileValidator()
        {
            RuleFor(x => x.FullName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Full name cannot be empty.")
                .MaximumLength(200).WithMessage("Full name cannot exceed 200 characters.");

            RuleFor(x => x.ContactNo)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Contact No. cannot be empty.")
                .Matches(@"^\d+$").WithMessage("Contact No. must contains only digit.")
                .MaximumLength(20).WithMessage("Contact No. cannot exceed 20 characters.");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid email format.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");
        }
    }
}
