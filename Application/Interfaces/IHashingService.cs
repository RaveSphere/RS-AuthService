using Core.Models;

namespace Application.Interfaces
{
    public interface IHashingService
    {
        public Task<HashingModel> Hash(string username, string password, Guid salt);
    }
}
