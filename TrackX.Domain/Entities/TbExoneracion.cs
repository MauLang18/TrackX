namespace TrackX.Domain.Entities;

public partial class TbExoneracion : BaseEntity
{
    public string Idtra { get; set; } = null!;

    public string Cliente { get; set; } = null!;

    public string TipoExoneracion { get; set; } = null!;

    public string StatusExoneracion { get; set; } = null!;

    public string Producto { get; set; } = null!;

    public string Categoria { get; set; } = null!;

    public string ClasificacionArancelaria { get; set; } = null!;

    public string? NumeroSolicitud { get; set; }

    public string? Solicitud { get; set; } = null!;

    public string? NumeroAutorizacion { get; set; } = null!;

    public string? Autorizacion { get; set; }

    public DateTime? Desde { get; set; }

    public DateTime? Hasta { get; set; }

    public string Descripcion { get; set; } = null!;

    public string? NombreCliente { get; set; }
}