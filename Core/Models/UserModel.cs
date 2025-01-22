namespace Core.Models
{
    public class UserModel
    {
        public string Username { get; init; }

        public UserModel(string username)
        {
            Username = username;
        }
    }
}
