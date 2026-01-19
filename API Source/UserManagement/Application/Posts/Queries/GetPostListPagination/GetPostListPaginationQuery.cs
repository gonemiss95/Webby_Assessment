using MediatR;
using UserManagement.Application.Posts.Queries.Dto;

namespace UserManagement.Application.Posts.Queries.GetPostListPagination
{
    public class GetPostListPaginationQuery : IRequest<PageResult<PostDto>>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int UserId { get; set; }
    }
}
