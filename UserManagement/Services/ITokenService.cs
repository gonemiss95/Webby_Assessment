using UserManagement.DbContext.Models;

namespace UserManagement.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
