using Newtonsoft.Json;

namespace TrackX.Domain.Entities
{
    public class Dynamics
    {
        [JsonProperty("@odata.context")]
        public string? odatacontext { get; set; }
        public List<Value>? value { get; set; }
    }

    public class Value
    {
        [JsonProperty("@odata.etag")]
        public string? odataetag { get; set; }
        public string? new_contenedor { get; set; }
        public string? new_bcf { get; set; }
        public DateTime? new_confirmacinzarpe { get; set; }
        public int new_destino { get; set; }
        public DateTime? new_eta { get; set; }
        public DateTime? new_etd1 { get; set; }
        public DateTime? new_instcliente { get; set; }
        public object? new_ingreso { get; set; }
        public object? new_ingresoabodegas { get; set; }
        public object? new_nombrebuque { get; set; }
        public int new_origen { get; set; }
        public int new_poe { get; set; }
        public int new_pol { get; set; }
        public int new_transporte { get; set; }
        public string? title { get; set; }
        public int new_preestado2 { get; set; }
        public DateTime? modifiedon { get; set; }
        public string? incidentid { get; set; }
    }
}