namespace TrackX.Application.Dtos.Logs.Response
{
    public class LogsResponseDto
    {
        public int Id { get; set; }
        public string? Usuario { get; set; }
        public string? Modulo { get; set; }
        public string? TipoMetodo { get; set; }
        public string? Parametros { get; set; }
        public DateTime FechaCreacionAuditoria { get; set; }
        public int Estado { get; set; }
        public string? EstadoLogs { get; set; }
    }
}