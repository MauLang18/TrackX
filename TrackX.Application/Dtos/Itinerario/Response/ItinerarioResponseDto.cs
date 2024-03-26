﻿using Microsoft.AspNetCore.Http;

namespace TrackX.Application.Dtos.Itinerario.Response
{
    public class ItinerarioResponseDto
    {
        public int Id { get; set; }
        public string? POL { get; set; }
        public string? POD { get; set; }
        public DateTime Closing { get; set; }
        public DateTime ETD { get; set; }
        public DateTime ETA { get; set; }
        public string? Carrier { get; set; }
        public string? Vessel { get; set; }
        public string? Voyage { get; set; }
        public string? Origen { get; set; }
        public string? Destino { get; set; }
        public string? Transporte { get; set; }
        public string? Modalidad { get; set; }
        public DateTime FechaCreacionAuditoria { get; set; }
        public int Estado { get; set; }
        public string? EstadoItinerario { get; set; }
    }
}