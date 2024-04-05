using Microsoft.AspNetCore.Http;

namespace TrackX.Application.Dtos.Whs.Request
{
    public class WhsRequestDto
    {
        public string? Idtra { get; set; }
        public string? NumeroWHS { get; set; }
        public string? Cliente { get; set; }
        public string? TipoRegistro { get; set; }
        public string? PO { get; set; }
        public string? StatusWHS { get; set; }
        public string? POL { get; set; }
        public string? POD { get; set; }
        public string? Detalle { get; set; }
        public string? CantidadBultos { get; set; }
        public string? TipoBultos { get; set; }
        public string? VinculacionOtroRegistro { get; set; }
        public IFormFile? WHSReceipt { get; set; }
        public IFormFile? Documentoregistro { get; set; }
        public IFormFile? Imagen1 { get; set; }
        public IFormFile? Imagen2 { get; set; }
        public IFormFile? Imagen3 { get; set; }
        public IFormFile? Imagen4 { get; set; }
        public IFormFile? Imagen5 { get; set; }
        public int Estado { get; set; }
    }
}