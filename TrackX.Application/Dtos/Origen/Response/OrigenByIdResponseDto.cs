namespace TrackX.Application.Dtos.Origen.Response;

public class OrigenByIdResponseDto
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Imagen { get; set; }
    public int Estado { get; set; }
}