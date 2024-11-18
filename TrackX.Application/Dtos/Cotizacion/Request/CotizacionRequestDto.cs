using Microsoft.AspNetCore.Http;

namespace TrackX.Application.Dtos.Cotizacion.Request;

public class CotizacionRequestDto
{
    public string? Quo { get; set; }
    public IFormFile? Cotizacion { get; set; }
    public string? Cliente { get; set; }
    public int Estado { get; set; }
}