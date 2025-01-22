namespace Api.DTO
{
    public class GetUserDTO
    {
        public string Username { get; init; }

        public GetUserDTO(string username)
        {
            Username = username;
        }
    }
}
