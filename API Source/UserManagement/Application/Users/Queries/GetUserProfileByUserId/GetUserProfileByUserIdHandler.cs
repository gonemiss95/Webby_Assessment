using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Users.Queries.Dto;
using UserManagement.DbContext;

namespace UserManagement.Application.Users.Queries.GetUserProfileByUserId
{
    public class GetUserProfileByUserIdHandler : IRequestHandler<GetUserProfileByUserIdQuery, UserProfileDto>
    {
        private readonly UserManagementDbContext _dbContext;

        public GetUserProfileByUserIdHandler(UserManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserProfileDto> Handle(GetUserProfileByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.UserProfiles
                .Where(x => x.UserId == request.UserId)
                .Select(x => new UserProfileDto()
                {
                    FullName = x.FullName,
                    Email = x.Email,
                    ContactNo = x.ContactNo
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
