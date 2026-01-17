using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.Common;
using UserManagement.DbContext;
using UserManagement.DbContext.Models;
using UserManagement.Services;

namespace UserManagement.Application.Users.Comands.LoginCommand
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly UserManagementDbContext _dbContext;
        private readonly ITokenService _tokenService;

        public LoginHandler(UserManagementDbContext dbContext, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }

        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            LoginResult result = new LoginResult();
            User user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken);

            if (user != null && PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                result.IsLoginSuccessful = true;
                result.JwtToken = _tokenService.GenerateToken(user);
                result.Message = "Login successful";
            }
            else
            {
                result.IsLoginSuccessful = false;
                result.Message = "Invalid username or password";
            }

            return result;
        }
    }
}
