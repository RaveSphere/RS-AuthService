using Api.DTO;
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
            return new CreateUserDTO(hashingModel.Username, hashingModel.HashedPassword, hashingModel.Salt);
        }
    }
}
