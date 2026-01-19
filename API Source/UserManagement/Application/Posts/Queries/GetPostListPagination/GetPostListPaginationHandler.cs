using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UserManagement.Application.Posts.Queries.Dto;
using UserManagement.Application.Tags.Queries.Dto;
using UserManagement.DbContext;
using UserManagement.Services;

namespace UserManagement.Application.Posts.Queries.GetPostListPagination
{
    public class GetPostListPaginationHandler : IRequestHandler<GetPostListPaginationQuery, PageResult<PostDto>>
    {
        private readonly UserManagementDbContext _dbContext;
        private readonly IRedisCacheService _cacheService;

        public GetPostListPaginationHandler(UserManagementDbContext dbContext, IRedisCacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        public async Task<PageResult<PostDto>> Handle(GetPostListPaginationQuery request, CancellationToken cancellationToken)
        {
            int total = await _dbContext.Posts.CountAsync(x => x.UserId == request.UserId, cancellationToken);
            List<PostDto> postList = await _cacheService.GetCache<List<PostDto>>($"post:{request.UserId}|{request.PageNumber}-{request.PageSize}");

            if (postList == null)
            {
                postList = await _dbContext.Posts
                    .Where(x => x.UserId == request.UserId)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
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

                if (postList.Count > 0)
                {
                    await _cacheService.SetCache($"post:{request.UserId}|{request.PageNumber}-{request.PageSize}", postList, TimeSpan.FromMinutes(10));
                }
            }

            return new PageResult<PostDto>()
            {
                TotalRecord = total,
                Result = postList
            };
        }
    }
}
