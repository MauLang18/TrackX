namespace TrackX.Application.Dtos.Cotizacion.Response;

public class CotizacionByIdResponseDto
{
    public int Id { get; set; }
    public string? Quo { get; set; }
    public string? Cotizacion { get; set; }
    public string? Cliente { get; set; }
    public int Estado { get; set; }
}