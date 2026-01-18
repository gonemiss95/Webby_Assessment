using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagement.Application;
using UserManagement.Application.Tags.Comands.CreateTagCommand;
using UserManagement.Application.Tags.Queries.Dto;
using UserManagement.Application.Tags.Queries.GetTagListPagination;

namespace UserManagement.Controllers
{
    [ApiController]
    [Route("api/tag")]
    public class TagController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("gettaglist")]
        public async Task<IActionResult> GetTagList([FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            PageResult<TagDto> result = await _mediator.Send(new GetTagListPaginationQuery()
            {
                PageNumber = pageNo,
                PageSize = pageSize
            });

            return Ok(result);
        }

        [HttpPost("createtag")]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagCommand createCmd)
        {
            createCmd.UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            CreateResult result = await _mediator.Send(createCmd);

            if (!result.IsCreateSuccessful)
            {
                return BadRequest(new
                {
                    message = result.Message
                });
            }
            else
            {
                return Ok(new
                {
                    message = result.Message
                });
            }
        }
    }
}
