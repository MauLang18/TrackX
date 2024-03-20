namespace TrackX.Application.Dtos.Empleo.Response
{
    public class EmpleoResponseDto
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Puesto { get; set; }
        public string? Descripcion { get; set; }
        public string? Imagen { get; set; }
        public DateTime FechaCreacionAuditoria { get; set; }
        public int Estado { get; set; }
        public string? EstadoEmpleo { get; set; }
    }
}