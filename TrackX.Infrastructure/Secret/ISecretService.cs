namespace TrackX.Infrastructure.Secret;

public interface ISecretService
{
    Task<string> GetSecret(string secretPath);
}