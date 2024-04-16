namespace TrackX.Domain.Entities
{
    public class TbPrivilegio : BaseEntity
    {
        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        public virtual ICollection<TbRolPrivilegio> TbRolPrivilegio { get; set; } = new List<TbRolPrivilegio>();
    }
}