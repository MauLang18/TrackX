namespace TrackX.Domain.Entities;

public partial class TbUsuario : BaseEntity
{
    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public string? Cliente { get; set; }

    public int IdRol { get; set; }

    public string? Imagen { get; set; }

    public string? NombreCliente { get; set; }

    public virtual TbRol IdRolNavigation { get; set; } = null!;
}