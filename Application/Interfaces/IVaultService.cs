namespace Application.Interfaces
{
    public interface IVaultService
    {
        Task<string?> GetSecretJsonAsync(string path);
    }
}
