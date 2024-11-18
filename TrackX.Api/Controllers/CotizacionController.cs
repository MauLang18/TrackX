using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Dtos.Cotizacion.Request;
using TrackX.Application.Interfaces;
using TrackX.Utilities.Static;

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
        public async Task<IActionResult> ListCotizacion([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _cotizacionApplication.ListCotizacion(filters);

            if ((bool)filters.Download!)
            {
                var columnNames = ExcelColumnNames.GetColumnsCotizacion();
                var fileBytes = _generateExcelApplication.GenerateToExcelGeneric(response.Data!, columnNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }

            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterCotizacion([FromForm] CotizacionRequestDto request)
        {
            var response = await _cotizacionApplication.RegisterCotizacion(request);

            return Ok(response);
        }
    }
}