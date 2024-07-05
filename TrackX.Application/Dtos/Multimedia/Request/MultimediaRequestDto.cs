using Microsoft.AspNetCore.Http;

namespace TrackX.Application.Dtos.Multimedia.Request;

public class MultimediaRequestDto
{
    public string? Nombre { get; set; }
    public IFormFile? Multimedia { get; set; }
    public int Estado { get; set; }
}