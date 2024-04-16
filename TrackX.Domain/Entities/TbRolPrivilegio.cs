namespace TrackX.Domain.Entities
{
    public class TbRolPrivilegio
    {
        public int Id { get; set; }

        public int IdRol { get; set; }

        public int IdPrivilegio { get; set; }

        public virtual TbRol Rol { get; set; } = null!;

        public virtual TbPrivilegio Privilegio { get; set; } = null!;
    }
}