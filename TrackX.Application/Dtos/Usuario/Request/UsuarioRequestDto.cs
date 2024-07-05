using Microsoft.AspNetCore.Http;

namespace TrackX.Application.Dtos.Usuario.Request;

public class UsuarioRequestDto
{
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Pass { get; set; }
    public string? Correo { get; set; }
    public string? Tipo { get; set; }
    public string? Cliente { get; set; }
    public int IdRol { get; set; }
    public IFormFile? Imagen { get; set; }
    public string? NombreEmpresa { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public string? Pais { get; set; }
    public string? Paginas { get; set; }
    public int Estado { get; set; }
}