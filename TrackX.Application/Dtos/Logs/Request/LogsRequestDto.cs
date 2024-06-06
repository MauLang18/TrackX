namespace TrackX.Application.Dtos.Logs.Request
{
    public class LogsRequestDto
    {
        public int Id { get; set; }
        public string? Usuario { get; set; }
        public string? Modulo { get; set; }
        public string? TipoMetodo { get; set; }
        public string? Parametros { get; set; }
        public int Estado { get; set; }
    }
}