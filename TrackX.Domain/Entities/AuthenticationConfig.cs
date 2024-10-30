namespace TrackX.Domain.Entities;

public class AuthenticationConfig
{
    public string? Authority { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? CrmUrl { get; set; }
}