namespace TrackX.Domain.Entities
{
    public class TbLogs : BaseEntity
    {
        public string Usuario { get; set; } = null!;
        public string Modulo { get; set; } = null!;
        public string TipoMetodo { get; set; } = null!;
        public string? Parametros { get; set; }
    }
}