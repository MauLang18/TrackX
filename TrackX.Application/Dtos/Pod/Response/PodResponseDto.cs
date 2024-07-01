namespace TrackX.Application.Dtos.Pod.Response
{
    public class PodResponseDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public DateTime FechaCreacionAuditoria { get; set; }
        public int Estado { get; set; }
        public string? EstadoPod { get; set; }
    }
}