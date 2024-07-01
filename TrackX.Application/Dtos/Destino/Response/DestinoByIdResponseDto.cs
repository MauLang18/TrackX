namespace TrackX.Application.Dtos.Destino.Response
{
    public class DestinoByIdResponseDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Imagen { get; set; }
        public int Estado { get; set; }
    }
}