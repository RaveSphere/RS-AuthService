namespace Api.ViewModels
{
    public class TokenVM
    {
        public string Token { get; init; }

        public TokenVM(string token)
        {
            Token = token;
        }
    }
}
