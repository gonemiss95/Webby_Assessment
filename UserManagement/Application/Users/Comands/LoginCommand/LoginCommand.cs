using MediatR;

namespace UserManagement.Application.Users.Comands.LoginCommand
{
    public class LoginCommand : IRequest<LoginResult>
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
