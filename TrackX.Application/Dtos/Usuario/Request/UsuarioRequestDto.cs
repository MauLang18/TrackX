using Microsoft.AspNetCore.Http;

namespace TrackX.Application.Dtos.Usuario.Request
{
    public class UsuarioRequestDto
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Pass { get; set; }
        public string? Correo { get; set; }
        public string? Tipo { get; set; }
        public string? Cliente { get; set; }
        public int Rol { get; set; }
    }
}