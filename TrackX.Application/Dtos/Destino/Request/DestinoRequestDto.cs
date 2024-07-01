using Microsoft.AspNetCore.Http;

namespace TrackX.Application.Dtos.Destino.Request
{
    public class DestinoRequestDto
    {
        public string? Nombre { get; set; }
        public IFormFile? Imagen { get; set; }
        public int Estado { get; set; }
    }
}