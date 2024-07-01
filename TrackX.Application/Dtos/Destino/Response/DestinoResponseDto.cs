namespace TrackX.Application.Dtos.Destino.Response
{
    public class DestinoResponseDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Imagen { get; set; }
        public DateTime FechaCreacionAuditoria { get; set; }
        public int Estado { get; set; }
        public string? EstadoDestino { get; set; }
    }
}