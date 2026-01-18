using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagement.Application;
using UserManagement.Application.Posts.Comands.CreatePostCommand;
using UserManagement.Application.Posts.Comands.UpdatePostCommand;
using UserManagement.Application.Posts.Queries.Dto;
using UserManagement.Application.Posts.Queries.GetPostListPagination;

namespace UserManagement.Controllers
{
    [ApiController]
    [Route("api/post")]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("getpostlist")]
        public async Task<IActionResult> GetPostList([FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            PageResult<PostDto> result = await _mediator.Send(new GetPostListPaginationQuery()
            {
                PageNumber = pageNo,
                PageSize = pageSize,
                UserId = userId
            });

            return Ok(result);
        }

        [HttpPost("createpost")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostCommand createCmd)
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

        [HttpPut("updatepost")]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostCommand updateCmd)
        {
            updateCmd.UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            UpdateResult result = await _mediator.Send(updateCmd);

            if (!result.IsUpdateSuccessful)
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
