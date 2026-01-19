using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.DbContext;
using UserManagement.DbContext.Models;

namespace UserManagement.Application.Users.Comands.UpdateUserProfileCommand
{
    public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileCommand, UpdateResult>
    {
        private readonly UserManagementDbContext _dbContext;

        public UpdateUserProfileHandler(UserManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UpdateResult> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            UpdateResult result = new UpdateResult();
            UserProfile profile = await _dbContext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

            if (profile != null)
            {
                profile.FullName = request.FullName;
                profile.ContactNo = request.ContactNo;
                profile.Email = request.Email;
                profile.UpdatedTimeStamp = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync(cancellationToken);

                result.IsUpdateSuccessful = true;
                result.Message = "User profile successfully updated.";
            }
            else
            {
                result.IsUpdateSuccessful = false;
                result.Message = "User profile not found.";
            }

            return result;
        }
    }
}
