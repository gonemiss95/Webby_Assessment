using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagement.Application;
using UserManagement.Application.Users.Comands.UpdateUserProfileCommand;
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
                return NotFound(new
                {
                    message = "User profile not found."
                });
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPut("updateuserprofile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileCommand updateCmd)
        {
            updateCmd.UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            UpdateResult result = await _mediator.Send(updateCmd);

            if (!result.IsUpdateSuccessful)
            {
                return NotFound(new
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
