namespace TrackX.Domain.Entities;

public partial class TbEmpleo : BaseEntity
{
    public string Titulo { get; set; } = null!;

    public string Puesto { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string Imagen { get; set; } = null!;
}