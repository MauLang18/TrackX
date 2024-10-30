namespace TrackX.Domain.Entities;

public class JwtConfig
{
    public string? Expires { get; set; }
    public string? Issuer { get; set; }
    public string? Secret { get; set; }
}