namespace TrackX.Application.Dtos.Cotizacion.Response;

public class CotizacionResponseDto
{
    public int Id { get; set; }
    public string? Quo { get; set; }
    public string? Cotizacion { get; set; }
    public string? Cliente { get; set; }
    public string? NombreCliente { get; set; }
    public DateTime FechaCreacionAuditoria { get; set; }
    public int Estado { get; set; }
    public string? EstadoCotizacion { get; set; }
}