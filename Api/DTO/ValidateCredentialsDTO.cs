namespace Api.DTO
{
    public class ValidateCredentialsDTO
    {
        public string Username { get; init; }
        public string Password { get; init; }

        public ValidateCredentialsDTO(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
