using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.Common;
using UserManagement.DbContext;
using UserManagement.DbContext.Models;

namespace UserManagement.Application.Users.Comands.RegisterCommand
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, CreateResult>
    {
        private readonly UserManagementDbContext _dbContext;

        public RegisterHandler(UserManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            CreateResult result = new CreateResult();
            User user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken);

            if (user != null)
            {
                result.IsCreateSuccessful = false;
                result.Message = "Username already exists.";
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

                result.IsCreateSuccessful = true;
                result.Message = "Register successfully.";
            }

            return result;
        }
    }
}
