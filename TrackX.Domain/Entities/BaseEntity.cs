namespace TrackX.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public int UsuarioCreacionAuditoria { get; set; }
        public DateTime FechaCreacionAuditoria { get; set; }
        public int? UsuarioActualizacionAuditoria { get; set; }
        public DateTime? FechaActualizacionAuditoria { get; set; }
        public int? UsuarioEliminacionAuditoria { get; set; }
        public DateTime? FechaEliminacionAuditoria { get; set; }
        public int Estado { get; set; }
    }
}