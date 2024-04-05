namespace TrackX.Application.Dtos.Whs.Response
{
    public class WhsResponseByIdDto
    {
        public int Id { get; set; }
        public string? Idtra { get; set; }
        public string? NumeroWHS { get; set; }
        public string? Cliente { get; set; }
        public string? TipoRegistro { get; set; }
        public string? PO { get; set; }
        public string? StatusWHS { get; set; }
        public string? POL { get; set; }
        public string? POD { get; set; }
        public string? Detalle { get; set; }
        public string? CantidadBultos { get; set; }
        public string? TipoBultos { get; set; }
        public string? VinculacionOtroRegistro { get; set; }
        public string? WHSReceipt { get; set; }
        public string? Documentoregistro { get; set; }
        public string? Imagen1 { get; set; }
        public string? Imagen2 { get; set; }
        public string? Imagen3 { get; set; }
        public string? Imagen4 { get; set; }
        public string? Imagen5 { get; set; }
        public int Estado { get; set; }
    }
}