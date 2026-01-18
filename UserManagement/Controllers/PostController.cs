using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagement.Application;
using UserManagement.Application.Posts.Comands.CreatePostCommand;

namespace UserManagement.Controllers
{
    [ApiController]
    [Route("api/tag")]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
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
    }
}
