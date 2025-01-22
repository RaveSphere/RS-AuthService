namespace Api.DTO
{
    public class CreateUserDTO
    {
        public string Username { get; init; }
        public string Password { get; init; }
        public Guid Salt { get; init; }

        public CreateUserDTO(string username, string password, Guid salt)
        {
            Username = username;
            Password = password;
            Salt = salt;
        }
    }
}
