namespace TrackX.Application.Dtos.Empleo.Response
{
    public class EmpleoByIdResponseDto
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Puesto { get; set; }
        public string? Descripcion { get; set; }
        public string? Imagen { get; set; }
        public int Estado { get; set; }
    }
}