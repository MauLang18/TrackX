using Microsoft.AspNetCore.Http;

namespace TrackX.Application.Dtos.Finance.Request
{
    public class FinanceRequestDto
    {
        public string? Cliente { get; set; }
        public IFormFile? EstadoCuenta { get; set; }
        public int Estado { get; set; }
    }
}