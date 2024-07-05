namespace TrackX.Application.Dtos.Multimedia.Response;

public class MultimediaResponseDto
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Multimedia { get; set; }
    public DateTime? FechaCreacionAuditoria { get; set; }
    public int Estado { get; set; }
    public string? EstadoMultimedia { get; set; }
}