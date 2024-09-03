using Newtonsoft.Json;

namespace TrackX.Domain.Entities;

public class DynamicsTransInternacional
{
    [JsonProperty("@odata.context")]
    public string? odatacontext { get; set; }
    public List<Value7>? value { get; set; }
}

public class Value7
{
    [JsonProperty("@odata.etag")]
    public string? odataetag { get; set; }
    public string? title { get; set; }
    //cliente
    //ejecutivo
    public string? new_preestado2 { get; set; }
    public int? new_pol { get; set; }
    public DateTime? new_confirmacinzarpe { get; set; }
    public int? new_poe { get; set; }
    //eta pto
    public string? new_contenedor { get; set; }
    public string? new_bcf { get; set; }
    public string? new_factura { get; set; }
    public string? new_po { get; set; }
    public string? new_contidadbultos { get; set; }
    public string? new_peso { get; set; }
    public int? new_cantequipo { get; set; }
    public int? new_tamaoequipo { get; set; }
    //fecha bl impreso
    //entrega bl original
    //libeeracion financiera
    //liberacion documental
    //entrega carta trazabilidad
    //fecha bl digitado tica
    //aplica certificado origen
    //aplica certificado re-exportacion
    public string? new_commodity { get; set; }
    //lleva exoneracion
    //entrega traduccion
}