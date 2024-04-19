using Newtonsoft.Json;

namespace TrackX.Domain.Entities
{
    public class DynamicsCreditoCliente
    {
        [JsonProperty("@odata.context")]
        public string? odatacontext { get; set; }
        public List<Value5>? value { get; set; }
    }

    public class Value5
    {
        [JsonProperty("@odata.etag")]
        public object? odataetag { get; set; }
        public object? _transactioncurrencyid_value { get; set; }
        public object? new_financiamiento { get; set; }
        public object? paymenttermscode { get; set; }
        public object? new_3 { get; set; }
        public object? new_creditonoincluye { get; set; }
        public object? new_diasdecredito { get; set; }
        public object? new_fechadeiniciodecredito { get; set; }
        public object? new_fechaderenovaciondecredito { get; set; }
        public object? new_intersmoratoriomensual { get; set; }
        public object? creditlimit { get; set; }
        public object? new_tipodeproveedor { get; set; }
        public object? accountid { get; set; }
    }
}