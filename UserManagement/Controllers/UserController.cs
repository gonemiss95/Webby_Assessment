using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagement.Application.Users.Queries.Dto;
using UserManagement.Application.Users.Queries.GetUserProfileByUserId;

namespace UserManagement.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("getuserprofile")]
        public async Task<IActionResult> GetUserProfile()
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            UserProfileDto result = await _mediator.Send(new GetUserProfileByUserIdQuery { UserId = userId });

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
