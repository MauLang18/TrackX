using Newtonsoft.Json;

namespace TrackX.Domain.Entities;

public class Dynamics<T> where T : class
{
    [JsonProperty("@odata.context")]
    public string? odatacontext { get; set; }
    public List<T>? value { get; set; }
}
