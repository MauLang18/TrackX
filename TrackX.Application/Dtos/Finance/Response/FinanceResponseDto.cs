namespace TrackX.Application.Dtos.Finance.Response;

public class FinanceResponseDto
{
    public int Id { get; set; }
    public string? Cliente { get; set; }
    public string? NombreCliente { get; set; }
    public string? EstadoCuenta { get; set; }
    public DateTime FechaCreacionAuditoria { get; set; }
    public int Estado { get; set; }
    public string? EstadoFinance { get; set; }
}