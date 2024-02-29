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
    }
}