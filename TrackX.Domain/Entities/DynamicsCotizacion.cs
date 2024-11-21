using Newtonsoft.Json;

namespace TrackX.Domain.Entities;

public class DynamicsCotizacion
{
    [JsonProperty("@odata.etag")]
    public object? odataetag { get; set; }
    public string? _customerid_value { get; set; }
    public object? quotenumber { get; set; }
    public object? new_servicioalcliente { get; set; }
    public object? new_almacenfiscal { get; set; }
    public object? new_consolidadoradecarga { get; set; }
    public object? new_clienteweb { get; set; }
    public object? new_enlacecotizacion { get; set; }
    public object? quoteid { get; set; }
}