namespace TrackX.Domain.Entities;

public class TbCotizacion : BaseEntity
{
    public string? QUO { get; set; }
    public string? Cotizacion { get; set; }
    public string? Cliente { get; set; }
    public string? NombreCliente { get; set; }
}