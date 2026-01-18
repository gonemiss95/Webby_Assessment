using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.Common;
using UserManagement.DbContext;
using UserManagement.DbContext.Models;

namespace UserManagement.Application.Users.Comands.RegisterCommand
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResult>
    {
        private readonly UserManagementDbContext _dbContext;

        public RegisterHandler(UserManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            RegisterResult result = new RegisterResult();
            User user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken);

            if (user != null)
            {
                result.IsRegisterSuccessful = false;
                result.Message = "Username is already occupied.";
            }
            else
            {
                DateTime dateUtcNow = DateTime.UtcNow;

                User newUser = new User()
                {
                    Username = request.Username,
                    PasswordHash = PasswordHasher.HashPassword(request.Password),
                    CreatedTimeStamp = dateUtcNow,
                    UpdatedTimeStamp = dateUtcNow,

                    UserProfiles = new List<UserProfile>()
                    {
                        new UserProfile()
                        {
                            FullName = request.FullName,
                            ContactNo = request.ContactNo,
                            Email = request.Email,
                            CreatedTimeStamp = dateUtcNow,
                            UpdatedTimeStamp = dateUtcNow
                        }
                    }
                };
                await _dbContext.Users.AddAsync(newUser, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                result.IsRegisterSuccessful = true;
                result.Message = "Register successfully.";
            }

            return result;
        }
    }
}
