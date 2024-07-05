using Newtonsoft.Json;

namespace TrackX.Domain.Entities
{
    public class DynamicsFacturas
    {
        [JsonProperty("@odata.context")]
        public string? odatacontext { get; set; }
        public List<Value6>? value { get; set; }
    }

    public class Value6
    {
        [JsonProperty("@odata.etag")]
        public string? odataetag { get; set; }
        public string? title { get; set; }
        public string? new_contenedor { get; set; }
        public string? _new_shipper_value { get; set; }
        public string? new_commodity { get; set; }
    }
}