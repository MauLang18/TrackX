using Microsoft.AspNetCore.Http;

namespace TrackX.Application.Dtos.Origen.Request;

public class OrigenRequestDto
{
    public string? Nombre { get; set; }
    public IFormFile? Imagen { get; set; }
    public int Estado { get; set; }
}