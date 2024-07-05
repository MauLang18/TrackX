namespace TrackX.Application.Dtos.Exoneracion.Response;

public class ExoneracionResponseDto
{
    public int Id { get; set; }
    public string? Idtra { get; set; }
    public string? Cliente { get; set; }
    public string? NombreCliente { get; set; }
    public string? TipoExoneracion { get; set; }
    public string? StatusExoneracion { get; set; }
    public string? Producto { get; set; }
    public string? Categoria { get; set; }
    public string? ClasificacionArancelaria { get; set; }
    public string? NumeroSolicitud { get; set; }
    public string? Solicitud { get; set; }
    public string? NumeroAutorizacion { get; set; }
    public string? Autorizacion { get; set; }
    public DateTime? Desde { get; set; }
    public DateTime? Hasta { get; set; }
    public string? Descripcion { get; set; }
    public DateTime FechaCreacionAuditoria { get; set; }
    public int Estado { get; set; }
    public string? EstadoExoneracion { get; set; }
}