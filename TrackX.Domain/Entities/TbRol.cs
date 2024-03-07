namespace TrackX.Domain.Entities;

public partial class TbRol : BaseEntity
{
    public string Nombre { get; set; } = null!;

    public virtual ICollection<TbUsuario> TbUsuarios { get; set; } = new List<TbUsuario>();
}