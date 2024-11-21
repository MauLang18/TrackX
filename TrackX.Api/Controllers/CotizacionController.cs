﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Dtos.Cotizacion.Request;
using TrackX.Application.Interfaces;

namespace TrackX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CotizacionController : ControllerBase
    {
        private readonly ICotizacionApplication _cotizacionApplication;
        private readonly IGenerateExcelApplication _generateExcelApplication;

        public CotizacionController(ICotizacionApplication cotizacionApplication, IGenerateExcelApplication generateExcelApplication)
        {
            _cotizacionApplication = cotizacionApplication;
            _generateExcelApplication = generateExcelApplication;
        }

        [HttpGet]
        public async Task<IActionResult> ListCotizacion(int numFilter = 0, string textFilter = null!)
        {
            var response = await _cotizacionApplication.ListCotizacion(numFilter, textFilter);

            return Ok(response);
        }

        [HttpPatch("Agregar")]
        public async Task<IActionResult> RegisterCotizacion([FromForm] CotizacionRequestDto request)
        {
            var response = await _cotizacionApplication.RegisterCotizacion(request);

            return Ok(response);
        }
    }
}