namespace TrackX.Application.Dtos.Origen.Response;

public class OrigenResponseDto
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Imagen { get; set; }
    public DateTime FechaCreacionAuditoria { get; set; }
    public int Estado { get; set; }
    public string? EstadoOrigen { get; set; }
}