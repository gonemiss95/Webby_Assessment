using MediatR;
using UserManagement.Application.Users.Queries.Dto;

namespace UserManagement.Application.Users.Queries.GetUserProfileByUserId
{
    public class GetUserProfileByUserIdQuery : IRequest<UserProfileDto>
    {
        public int UserId { get; set; }
    }
}
