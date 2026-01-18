using MediatR;

namespace UserManagement.Application.Users.Comands.RegisterCommand
{
    public class RegisterCommand : IRequest<CreateResult>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string FullName { get; set; }

        public string ContactNo { get; set; }

        public string Email { get; set; }
    }
}
