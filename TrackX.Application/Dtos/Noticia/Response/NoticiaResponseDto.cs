﻿namespace TrackX.Application.Dtos.Noticia.Response;

public class NoticiaResponseDto
{
    public int Id { get; set; }
    public string? Titulo { get; set; }
    public string? Subtitulo { get; set; }
    public string? Contenido { get; set; }
    public string? Imagen { get; set; }
    public DateTime FechaCreacionAuditoria { get; set; }
    public int Estado { get; set; }
    public string? EstadoNoticia { get; set; }
}