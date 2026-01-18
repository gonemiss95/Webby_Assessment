using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Tags.Queries.Dto;
using UserManagement.DbContext;

namespace UserManagement.Application.Tags.Queries.GetTagListPagination
{
    public class GetTagListPaginationHandler : IRequestHandler<GetTagListPaginationQuery, PageResult<TagDto>>
    {
        private readonly UserManagementDbContext _dbContext;

        public GetTagListPaginationHandler(UserManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PageResult<TagDto>> Handle(GetTagListPaginationQuery request, CancellationToken cancellationToken)
        {
            int total = await _dbContext.Tags.CountAsync(cancellationToken);
            List<TagDto> tagList = await _dbContext.Tags
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new TagDto()
                {
                    TagName = x.TagName,
                    TagDesctiption = x.TagDescription
                })
                .ToListAsync(cancellationToken);

            return new PageResult<TagDto>()
            {
                TotalRecord = total,
                Result = tagList
            };
        }
    }
}
