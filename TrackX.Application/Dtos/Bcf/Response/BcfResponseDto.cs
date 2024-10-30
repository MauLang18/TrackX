namespace TrackX.Application.Dtos.Bcf.Response;

public class BcfResponseDto
{
    public int Id { get; set; }
    public string? Idtra { get; set; }
    public string? Bcf { get; set; }
    public string? Cliente { get; set; }
    public string? NombreCliente { get; set; }
    public DateTime FechaCreacionAuditoria { get; set; }
    public int Estado { get; set; }
    public string? EstadoBcf { get; set; }
}