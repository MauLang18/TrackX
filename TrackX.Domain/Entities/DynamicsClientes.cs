using Newtonsoft.Json;

namespace TrackX.Domain.Entities;

public class DynamicsClientes
{
    [JsonProperty("@odata.etag")]
    public string? odataetag { get; set; }
    public string? name { get; set; }
    public string? accountid { get; set; }
}