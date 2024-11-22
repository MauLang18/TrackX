using Microsoft.AspNetCore.Http;

namespace TrackX.Application.Dtos.TransInternacional.Request;

public class TransInternacionalRequestDto
{
    public string? TransInternacionalId { get; set; }
    public string? FieldName { get; set; }
    public string? Comentario { get; set; }
}

public class TransInternacionalDocumentRequestDto
{
    public string? TransInternacionalId { get; set; }
    public string? FieldName { get; set; }
    public IFormFile? File { get; set; }
}