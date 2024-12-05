using Microsoft.AspNetCore.Http;

namespace TrackX.Application.Dtos.Panama.Request;

public class PanamaRequestDto
{
    public string? PanamaId { get; set; }
    public string? FieldName { get; set; }
    public string? Comentario { get; set; }
}

public class PanamaDocumentRequestDto
{
    public string? PanamaId { get; set; }
    public string? FieldName { get; set; }
    public IFormFile? File { get; set; }
}

public class PanamaRemoveDocumentRequestDto
{
    public string? PanamaId { get; set; }
    public string? FieldName { get; set; }
    public string? FileUrl { get; set; }
}