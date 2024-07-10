using Microsoft.AspNetCore.Http;

namespace TrackX.Application.Dtos.Itinerario.Request
{
    public class ImportItinerarioRequest
    {
        public IFormFile? excel { get; set; }
    }
}