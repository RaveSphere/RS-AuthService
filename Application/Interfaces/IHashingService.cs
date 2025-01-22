using Core.Models;

namespace Application.Interfaces
{
    public interface IHashingService
    {
        Task<HashingModel> HashAsync(string username, string password, Guid salt);
    }
}
