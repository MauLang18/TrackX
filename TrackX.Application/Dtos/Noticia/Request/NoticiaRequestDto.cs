using Microsoft.AspNetCore.Http;

namespace TrackX.Application.Dtos.Noticia.Request
{
    public class NoticiaRequestDto
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Subtitulo { get; set; }
        public string? Contenido { get; set; }
        public IFormFile? Imagen { get; set; }
        public int Estado { get; set; }
    }
}