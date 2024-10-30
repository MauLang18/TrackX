
using System.Net.Http.Headers;

namespace TrackX.Infrastructure.Secret;

public class VaultSecretService : ISecretService
{
    public async Task<string> GetSecret(string secretPath)
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri("https://vault.customcodecr.com");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "root");

            var response = await httpClient.GetAsync($"/v1/{secretPath}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return json;
        }
    }
}