namespace TrackX.Domain.Entities;

public partial class TbWhs : BaseEntity
{
    public string Idtra { get; set; } = null!;

    public string Cliente { get; set; } = null!;

    public string TipoRegistro { get; set; } = null!;

    public string PO { get; set; } = null!;

    public string StatusWHS { get; set; } = null!;

    public string POL { get; set; } = null!;

    public string POD { get; set; } = null!;

    public string Detalle { get; set; } = null!;

    public string CantidadBultos { get; set; } = null!;

    public string TipoBultos { get; set; } = null!;

    public string? VinculacionOtroRegistro { get; set; }

    public string WHSReceipt { get; set; } = null!;

    public string? Documentoregistro { get; set; }

    public string Imagen1 { get; set; } = null!;

    public string? Imagen2 { get; set; }

    public string? Imagen3 { get; set; }

    public string? Imagen4 { get; set; }

    public string? Imagen5 { get; set; }

    public string? NumeroWHS { get; set; }

    public string? NombreCliente { get; set; }
}