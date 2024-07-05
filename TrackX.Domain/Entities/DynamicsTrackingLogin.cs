using Newtonsoft.Json;

namespace TrackX.Domain.Entities;

public class DynamicsTrackingLogin
{
    [JsonProperty("@odata.context")]
    public string? odatacontext { get; set; }
    public List<Value2>? value { get; set; }
}

public class Value2
{
    [JsonProperty("@odata.etag")]
    public string? odataetag { get; set; }
    public string? new_contenedor { get; set; }
    public string? new_factura { get; set; }
    public string? new_bcf { get; set; }
    public int? new_cantequipo { get; set; }
    public string? new_commodity { get; set; }
    public DateTime? new_confirmacinzarpe { get; set; }
    public string? new_contidadbultos { get; set; }
    public int new_destino { get; set; }
    public DateTime? new_eta { get; set; }
    public DateTime? new_etd1 { get; set; }
    public DateTime? modifiedon { get; set; }
    public int? new_incoterm { get; set; }
    public int? new_origen { get; set; }
    public string? new_po { get; set; }
    public int? new_poe { get; set; }
    public int? new_pol { get; set; }
    public int? new_preestado2 { get; set; }
    public string? new_seal { get; set; }
    public string? _new_shipper_value { get; set; }
    public string? new_statuscliente { get; set; }
    public int? new_tamaoequipo { get; set; }
    public string? new_new_facturacompaia { get; set; }
    public int? new_transporte { get; set; }
    public string? title { get; set; }
    public object? new_lugarcolocacion { get; set; }
    public object? new_redestino { get; set; }
    public int? new_diasdetransito { get; set; }
    public object? new_barcodesalida { get; set; }
    public object? new_viajedesalida { get; set; }
    public string? _customerid_value { get; set; }
    public object? new_ofertatarifaid { get; set; }
    public object? new_proyecciondeingreso { get; set; }
    public string? incidentid { get; set; }
}