using Microsoft.AspNetCore.Http;

namespace TrackX.Application.Commons.Bases.Request;

public class ImportRequest
{
    public IFormFile? excel { get; set; }
}