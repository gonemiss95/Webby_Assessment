using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Posts.Queries.Dto;
using UserManagement.Application.Tags.Queries.Dto;
using UserManagement.DbContext;

namespace UserManagement.Application.Posts.Queries.GetPostListPagination
{
    public class GetPostListPaginationHandler : IRequestHandler<GetPostListPaginationQuery, PageResult<PostDto>>
    {
        private readonly UserManagementDbContext _dbContext;

        public GetPostListPaginationHandler(UserManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PageResult<PostDto>> Handle(GetPostListPaginationQuery request, CancellationToken cancellationToken)
        {
            int total = await _dbContext.Posts.CountAsync(x => x.UserId == request.UserId, cancellationToken);
            List<PostDto> postList = await _dbContext.Posts
                .Where(x => x.UserId == request.UserId)
                .Select(x => new PostDto()
                {
                    PostAbbr = x.PostAbbr,
                    PostTitle = x.PostTitle,
                    TagList = x.PostTags
                        .Select(y => new TagDto()
                        {
                            TagName = y.Tag.TagName,
                            TagDesctiption = y.Tag.TagDescription
                        })
                        .ToList()
                })
                .ToListAsync(cancellationToken);


            return new PageResult<PostDto>()
            {
                TotalRecord = total,
                Result = postList
            };
        }
    }
}
