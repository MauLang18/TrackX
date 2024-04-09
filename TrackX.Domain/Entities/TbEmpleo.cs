namespace TrackX.Domain.Entities;

public partial class TbEmpleo : BaseEntity
{
    public string? Titulo { get; set; }

    public string? Puesto { get; set; }

    public string? Descripcion { get; set; }

    public string? Imagen { get; set; }
}