using Newtonsoft.Json;

namespace TrackX.Domain.Entities;

public class DynamicsClientes
{
    [JsonProperty("@odata.context")]
    public string? odatacontext { get; set; }
    public List<Value3>? value { get; set; }
}

public class Value3
{
    [JsonProperty("@odata.etag")]
    public string? odataetag { get; set; }
    public string? name { get; set; }
    public string? accountid { get; set; }
}