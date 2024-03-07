namespace TrackX.Application.Dtos.Usuario.Response
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Correo { get; set; }
        public string? Tipo { get; set; }
        public string? Cliente { get; set; }
        public string? Rol { get; set; }
        public int IdRol { get; set; }
        public DateTime FechaCreacionAuditoria { get; set; }
        public int Estado { get; set; }
        public string? EstadoUsuario { get; set; }
    }
}