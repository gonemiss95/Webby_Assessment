using MediatR;
using UserManagement.Application.Tags.Queries.Dto;

namespace UserManagement.Application.Tags.Queries.GetTagListPagination
{
    public class GetTagListPaginationQuery : IRequest<PageResult<TagDto>>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
