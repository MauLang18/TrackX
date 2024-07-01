namespace TrackX.Application.Dtos.Pol.Response
{
    public class PolResponseDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int WHS { get; set; }
        public string? EstadoWHS { get; set; }
        public int Estado { get; set; }
        public string? EstadoPol { get; set; }
    }
}