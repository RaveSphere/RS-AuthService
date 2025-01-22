using Core.Models;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(UserModel user);
    }
}
