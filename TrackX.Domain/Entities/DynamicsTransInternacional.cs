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
    public string? _customerid_value { get; set; }
    public string? new_ejecutivocomercial { get; set; }
    public string? new_preestado2 { get; set; }
    public int? new_pol { get; set; }
    public DateTime? new_confirmacinzarpe { get; set; }
    public int? new_poe { get; set; }
    public DateTime? new_eta { get; set; }
    public DateTime? new_etadestino { get; set; }
    public string? new_contenedor { get; set; }
    public string? new_bcf { get; set; }
    public string? new_factura { get; set; }
    public string? new_po { get; set; }
    public string? new_contidadbultos { get; set; }
    public string? new_peso { get; set; }
    public int? new_cantequipo { get; set; }
    public int? new_tamaoequipo { get; set; }
    public object? new_fechablimpreso { get; set; }
    public object? new_entregabloriginal { get; set; }
    public object? new_fechaliberacionfinanciera { get; set; }
    public object? new_liberacionmovimientoinventario { get; set; }
    public object? new_entregacartatrazabilidad { get; set; }
    public object? new_fechabldigittica { get; set; }
    //aplica certificado origen
    //aplica certificado re-exportacion
    public string? new_commodity { get; set; }
    public object? new_llevaexoneracion { get; set; }
    public object? new_entregatraduccion { get; set; }
}