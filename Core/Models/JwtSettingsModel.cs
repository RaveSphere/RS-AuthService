namespace Core.Models
{
    public class JwtSettingsModel
    {
        public required string Key { get; init; }
        public required string Issuer { get; init; }
        public required string Audience { get; init; }
    }
}
