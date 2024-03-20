using Microsoft.AspNetCore.Http;

namespace TrackX.Application.Dtos.Empleo.Request
{
    public class EmpleoRequestDto
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Puesto { get; set; }
        public string? Descripcion { get; set; }
        public IFormFile? Imagen { get; set; }
        public int Estado { get; set; }
    }
}