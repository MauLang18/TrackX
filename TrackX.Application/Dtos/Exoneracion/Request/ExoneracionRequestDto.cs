using Microsoft.AspNetCore.Http;

namespace TrackX.Application.Dtos.Exoneracion.Request
{
    public class ExoneracionRequestDto
    {
        public string? Idtra { get; set; }
        public string? Cliente { get; set; }
        public string? TipoExoneracion { get; set; }
        public string? StatusExoneracion { get; set; }
        public string? StatusWHS { get; set; }
        public string? Producto { get; set; }
        public string? Categoria { get; set; }
        public string? ClasificacionArancelaria { get; set; }
        public string? NumeroSolicitud { get; set; }
        public IFormFile? Solicitud { get; set; }
        public string? NumeroAutorizacion { get; set; }
        public IFormFile? Autorizacion { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        public string? Descripcion { get; set; }
        public int Estado { get; set; }
    }
}