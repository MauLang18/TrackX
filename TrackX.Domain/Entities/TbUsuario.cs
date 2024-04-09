namespace TrackX.Domain.Entities;

public partial class TbUsuario : BaseEntity
{
    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Correo { get; set; }

    public string? Pass { get; set; }

    public string? Tipo { get; set; }

    public string? Cliente { get; set; }

    public int IdRol { get; set; }

    public string? Imagen { get; set; }

    public string? NombreCliente { get; set; }

    public string? NombreEmpresa { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string? Pais { get; set; }

    public string? Paginas { get; set; }

    public virtual TbRol IdRolNavigation { get; set; } = null!;
}