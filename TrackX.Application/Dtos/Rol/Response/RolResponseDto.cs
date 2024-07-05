namespace TrackX.Application.Dtos.Rol.Response;

public class RolResponseDto
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public DateTime FechaCreacionAuditoria { get; set; }
    public int Estado { get; set; }
    public string? EstadoRol { get; set; }
}