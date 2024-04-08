namespace TrackX.Domain.Entities;

public partial class TbFinance : BaseEntity
{
    public string Cliente { get; set; } = null!;

    public string EstadoCuenta { get; set; } = null!;

    public string? NombreCliente { get; set; }
}