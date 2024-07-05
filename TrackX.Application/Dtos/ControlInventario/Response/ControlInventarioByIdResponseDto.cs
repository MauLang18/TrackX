namespace TrackX.Application.Dtos.ControlInventario.Response
{
    public class ControlInventarioByIdResponseDto
    {
        public int Id { get; set; }
        public string? Cliente { get; set; }
        public string? ControlInventario { get; set; }
        public string? Pol { get; set; }
        public int Estado { get; set; }
    }
}