using Newtonsoft.Json;

namespace TrackX.Domain.Entities;

public class DynamicsIdtraClientes
{
    [JsonProperty("@odata.etag")]
    public string? odataetag { get; set; }
    public string? title { get; set; }
    public string? _customerid_value { get; set; }
    public string? incidentid { get; set; }
}