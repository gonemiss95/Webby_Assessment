using FluentValidation;

namespace UserManagement.Application.Users.Comands.LoginCommand
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Username cannot be empty.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 character.");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .MaximumLength(50).WithMessage("Password cannot exceed 50 character.");
        }
    }
}
