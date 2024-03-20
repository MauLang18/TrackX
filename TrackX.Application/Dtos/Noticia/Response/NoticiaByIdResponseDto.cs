namespace TrackX.Application.Dtos.Noticia.Response
{
    public class NoticiaByIdResponseDto
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Subtitulo { get; set; }
        public string? Contenido { get; set; }
        public string? Imagen { get; set; }
        public int Estado { get; set; }
    }
}