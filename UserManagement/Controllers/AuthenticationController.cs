using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Users.Comands.LoginCommand;
using UserManagement.Application.Users.Comands.RegisterCommand;

namespace UserManagement.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommand loginCmd)
        {
            LoginResult result = await _mediator.Send(loginCmd);

            if (!result.IsLoginSuccessful)
            {
                return Unauthorized(new
                {
                    message = result.Message
                });
            }
            else
            {
                return Ok(new
                {
                    jwtToken = result.JwtToken,
                    message = result.Message
                });
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterCommand registerCmd)
        {
            RegisterResult result = await _mediator.Send(registerCmd);

            if (!result.IsRegisterSuccessful)
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
