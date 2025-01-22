namespace Api.DTO
{
    public class GetUserSaltDTO
    {
        public string Username { get; init; }

        public GetUserSaltDTO(string username)
        {
            Username = username;
        }
    }
}
