namespace TrackX.Domain.Entities
{
    public class TbControlInventarioWhs : BaseEntity
    {
        public string? Cliente { get; set; }
        public string? NombreCliente { get; set; }
        public string? ControlInventario { get; set; }
        public string? Pol { get; set; }
    }
}