using Core.Models;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        public Task<TokenModel> GenerateToken();
    }
}
