using Microsoft.AspNetCore.Http;

namespace TrackX.Application.Dtos.ControlInventario.Request
{
    public class ControlInventarioRequestDto
    {
        public string? Cliente { get; set; }
        public IFormFile? ControlInventario { get; set; }
        public string? Pol { get; set; }
        public int Estado { get; set; }
    }
}