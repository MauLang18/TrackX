namespace TrackX.Domain.Entities;

public partial class TbNoticia : BaseEntity
{
    public string? Titulo { get; set; }

    public string? Subtitulo { get; set; }

    public string? Contenido { get; set; }

    public string? Imagen { get; set; }
}