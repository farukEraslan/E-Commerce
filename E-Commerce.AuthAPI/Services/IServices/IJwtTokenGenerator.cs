using E_Commerce.AuthAPI.Models;

namespace E_Commerce.AuthAPI.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(AppUser user, IList<string> roles);
    }
}
