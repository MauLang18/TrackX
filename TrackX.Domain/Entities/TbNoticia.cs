namespace TrackX.Domain.Entities;

public partial class TbNoticia : BaseEntity
{
    public string Titulo { get; set; } = null!;

    public string Subtitulo { get; set; } = null!;

    public string Contenido { get; set; } = null!;

    public string Imagen { get; set; } = null!;
}