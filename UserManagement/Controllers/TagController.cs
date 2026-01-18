using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagement.Application.Tags.Comands.CreateTagCommand;

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

        [HttpPost("createtag")]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagCommand createCmd)
        {
            createCmd.UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            CreateTagResult result = await _mediator.Send(createCmd);

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
