using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Tags.Queries.Dto;
using UserManagement.DbContext;
using UserManagement.Services;

namespace UserManagement.Application.Tags.Queries.GetTagListPagination
{
    public class GetTagListPaginationHandler : IRequestHandler<GetTagListPaginationQuery, PageResult<TagDto>>
    {
        private readonly UserManagementDbContext _dbContext;
        private readonly IRedisCacheService _cacheService;

        public GetTagListPaginationHandler(UserManagementDbContext dbContext, IRedisCacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        public async Task<PageResult<TagDto>> Handle(GetTagListPaginationQuery request, CancellationToken cancellationToken)
        {
            int total = await _dbContext.Tags.CountAsync(cancellationToken);
            List<TagDto> tagList = await _cacheService.GetCache<List<TagDto>>($"tag:{request.PageNumber}-{request.PageSize}");

            if (tagList == null)
            {
                tagList = await _dbContext.Tags
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new TagDto()
                    {
                        TagName = x.TagName,
                        TagDesctiption = x.TagDescription
                    })
                    .ToListAsync(cancellationToken);

                if (tagList.Count > 0)
                {
                    await _cacheService.SetCache($"tag:{request.PageNumber}-{request.PageSize}", tagList, TimeSpan.FromMinutes(10));
                }
            }

            return new PageResult<TagDto>()
            {
                TotalRecord = total,
                Result = tagList
            };
        }
    }
}
