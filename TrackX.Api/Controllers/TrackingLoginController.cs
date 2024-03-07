using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Interfaces;
using TrackX.Application.Services;
using TrackX.Infrastructure.Commons.Bases.Request;
using TrackX.Utilities.Static;

namespace TrackX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingLoginController : ControllerBase
    {
        private readonly ITrackingLoginApplication _trackingLoginApplication;
        private readonly IGenerateExcelApplication _generateExcelApplication;

        public TrackingLoginController(ITrackingLoginApplication trackingLoginApplication, IGenerateExcelApplication generateExcelApplication)
        {
            _trackingLoginApplication = trackingLoginApplication;
            _generateExcelApplication = generateExcelApplication;
        }

        [HttpGet("Activo")]
        public async Task<IActionResult> TrackingActivoByCliente (string cliente)
        {
            var response = await _trackingLoginApplication.TrackingActivoByCliente(cliente);

            /*if ((bool)filters.Download!)
            {
                var columnNames = ExcelColumnNames.GetColumnsCategorias();
                var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!, columnNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }*/

            return Ok(response);
        }

        [HttpGet("Finalizado")]
        public async Task<IActionResult> TrackingFinalizadoByCliente(string cliente)
        {
            var response = await _trackingLoginApplication.TrackingFinalizadoByCliente(cliente);

            /*if ((bool)filters.Download!)
            {
                var columnNames = ExcelColumnNames.GetColumnsCategorias();
                var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!, columnNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }*/

            return Ok(response);
        }
    }
}