namespace TrackX.Domain.Entities;

public partial class TbFinance : BaseEntity
{
    public string? Cliente { get; set; }

    public string? EstadoCuenta { get; set; }

    public string? NombreCliente { get; set; }
}