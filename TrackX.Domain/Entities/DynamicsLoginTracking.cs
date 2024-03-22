using Newtonsoft.Json;

namespace TrackX.Domain.Entities
{
    public class DynamicsLoginTracking
    {
        [JsonProperty("@odata.context")]
        public string? odatacontext { get; set; }
        public List<Value4>? value { get; set; }
    }

    public class Value4
    {
        [JsonProperty("@odata.etag")]
        public string? odataetag { get; set; }
        public object? new_contenedor { get; set; }
        public string? new_factura { get; set; }
        public object? new_bcf { get; set; }
        public int? new_cantequipo { get; set; }
        public string? new_commodity { get; set; }
        public object? new_confirmacinzarpe { get; set; }
        public string? new_contidadbultos { get; set; }
        public int? new_destino { get; set; }
        public object? new_eta { get; set; }
        public object? new_etd1 { get; set; }
        public DateTime? modifiedon { get; set; }
        public int? new_incoterm { get; set; }
        public int? new_origen { get; set; }
        public string? new_po { get; set; }
        public int? new_poe { get; set; }
        public int? new_pol { get; set; }
        public int? new_preestado2 { get; set; }
        public object? new_seal { get; set; }
        public string? _new_shipper_value { get; set; }
        public object? new_statuscliente { get; set; }
        public int? new_tamaoequipo { get; set; }
        public int? new_transporte { get; set; }
        public object? new_ingreso { get; set; }
        public string? new_new_facturacompaia { get; set; }
        public object? new_ingresoabodegas { get; set; }
        public object? new_instcliente { get; set; }
        public object? new_barcodesalida { get; set; }
        public string? title { get; set; }
        public string? incidentid { get; set; }
    }
}