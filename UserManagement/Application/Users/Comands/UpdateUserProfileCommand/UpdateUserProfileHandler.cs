using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.DbContext;
using UserManagement.DbContext.Models;

namespace UserManagement.Application.Users.Comands.UpdateUserProfileCommand
{
    public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileCommand, bool>
    {
        private readonly UserManagementDbContext _dbContext;

        public UpdateUserProfileHandler(UserManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            UserProfile profile = await _dbContext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

            if (profile != null)
            {
                profile.FullName = request.FullName;
                profile.ContactNo = request.ContactNo;
                profile.Email = request.Email;
                profile.UpdatedTimeStamp = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
