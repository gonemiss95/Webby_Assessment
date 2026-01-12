using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Model;
using UserManagement.Services;

namespace UserManagement.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthenticationController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginViewModel loginVM)
        {
            try
            {
                if (loginVM.Username != "admin" && loginVM.Password != "admin123")
                {
                    return Unauthorized("Wrong username or password");
                }

                return Ok(new
                {
                    jwtToken = _tokenService.GenerateToken(loginVM.Username)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
