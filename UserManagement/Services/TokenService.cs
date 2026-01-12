using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UserManagement.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(string username)
        {
            List<Claim> claimList = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            IConfigurationSection jwtSettings = _config.GetSection("Jwt");
            SymmetricSecurityKey secuKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtSettings["Key"]));
            SigningCredentials signCreds = new SigningCredentials(secuKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken jwtToken = new JwtSecurityToken(jwtSettings["Issuer"], jwtSettings["Audience"], claimList, null, DateTime.UtcNow.AddMinutes(60), signCreds);

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
