using Api.DTO;
using Api.RequestModels;
using Api.ResponseModels;
using Api.ViewModels;
using Core.Models;

namespace Api.Mappers
{
    internal class UserMapper
    {
        public static UserVM Map(CreateUserResponse user)
        {
            return new UserVM(user.Username);
        }

        public static CreateUserDTO Map(HashingModel hashingModel)
        {
            return new CreateUserDTO(hashingModel.Username, hashingModel.Password, hashingModel.Salt);
        }

        public static GetUserDTO Map(LoginRequest request)
        {
            return new GetUserDTO(request.Username);
        }

        public static GetUserSaltDTO SaltMap(LoginRequest request)
        {
            return new GetUserSaltDTO(request.Username);
        }
    }
}
