namespace TrackX.Application.Dtos.Bcf.Response;

public class BcfByIdResponseDto
{
    public int Id { get; set; }
    public string? Idtra { get; set; }
    public string? Bcf { get; set; }
    public string? Cliente { get; set; }
    public string? NombreCliente { get; set; }
    public int Estado { get; set; }
}