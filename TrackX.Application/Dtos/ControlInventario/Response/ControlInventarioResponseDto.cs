namespace TrackX.Application.Dtos.ControlInventario.Response
{
    public class ControlInventarioResponseDto
    {
        public int Id { get; set; }
        public string? Cliente { get; set; }
        public string? NombreCliente { get; set; }
        public string? ControlInventario { get; set; }
        public string? Pol { get; set; }
        public DateTime FechaCreacionAuditoria { get; set; }
        public int Estado { get; set; }
        public string? EstadoControlInventario { get; set; }
    }
}