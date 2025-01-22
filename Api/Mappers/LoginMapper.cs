using Api.DTO;
using Api.RequestModels;
using Api.ResponseModels;
using Core.Models;

namespace Api.Mappers
{
    internal class LoginMapper
    {
        public static GetUserSaltDTO SaltMap(LoginRequest request)
        {
            return new GetUserSaltDTO(request.Username);
        }

        public static ValidateCredentialsDTO Map(HashingModel hashingModel)
        {
            return new ValidateCredentialsDTO(hashingModel.Username, hashingModel.Password);
        }

        public static UserModel Map(GetUserResponse user)
        {
            return new UserModel(user.Username);
        }
    }
}
