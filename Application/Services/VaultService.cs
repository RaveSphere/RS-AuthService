using Application.Interfaces;
using System.Text.Json;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Kubernetes;
using VaultSharp.V1.Commons;

namespace Application.Services
{
    public class VaultService : IVaultService
    {
        private readonly IVaultClient _vaultClient;

        public VaultService(string? vaultAddress, string? vaultRole, string? jwt)
        {
            KubernetesAuthMethodInfo authMethod = new KubernetesAuthMethodInfo(vaultRole, jwt);
            VaultClientSettings vaultClientSettings = new VaultClientSettings(vaultAddress, authMethod);
            _vaultClient = new VaultClient(vaultClientSettings);
        }

        public async Task<string?> GetSecretJsonAsync(string path)
        {
            Secret<SecretData> kvSecrets = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path, mountPoint: "kv");
            return JsonSerializer.Serialize(kvSecrets.Data.Data);
        }
    }
}
