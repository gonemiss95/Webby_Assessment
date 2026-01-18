using FluentValidation;

namespace UserManagement.Application.Users.Comands.RegisterCommand
{
    public class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Username)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Username cannot be empty.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 character.");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[~!@#$%^&*()_+`\-=[\]{},./<>?:""|;'\\]).{8,}$")
                    .WithMessage("Password must contains at least 8 characters, one lowercase, one uppercase, one digit and one special character.")
                .MaximumLength(50).WithMessage("Password cannot exceed 50 character.");

            RuleFor(x => x.ConfirmPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Confirm password cannot be empty.")
                .Equal(x => x.Password).WithMessage("Confirm password and password must be same.");

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
