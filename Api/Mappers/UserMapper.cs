using Api.ResponseModels;
using Api.ViewModels;

namespace Api.Mappers
{
    internal class UserMapper
    {
        public static UserVM Map(CreateUserResponse user)
        {
            return new UserVM(user.Username);
        }
    }
}
