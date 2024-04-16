namespace TrackX.Domain.Entities;

public partial class TbRol : BaseEntity
{
    public string? Nombre { get; set; }

    public virtual ICollection<TbUsuario> TbUsuarios { get; set; } = new List<TbUsuario>();
    public virtual ICollection<TbRolPrivilegio> TbRolPrivilegio { get; set; } = new List<TbRolPrivilegio>();
}