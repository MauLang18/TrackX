namespace TrackX.Application.Dtos.Finance.Response
{
    public class FinanceByIdResponeDto
    {
        public int Id { get; set; }
        public string? Cliente { get; set; }
        public string? EstadoCuenta { get; set; }
        public int Estado { get; set; }
    }
}