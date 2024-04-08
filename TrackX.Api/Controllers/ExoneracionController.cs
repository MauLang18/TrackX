using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Dtos.Exoneracion.Request;
using TrackX.Application.Interfaces;
using TrackX.Application.Services;

namespace TrackX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExoneracionController : ControllerBase
    {
        private readonly IExoneracionApplication _exoneracionApplication;
        private readonly IGenerateExcelApplication _generateExcelApplication;

        public ExoneracionController(IExoneracionApplication exoneracionApplication, IGenerateExcelApplication generateExcelApplication)
        {
            _exoneracionApplication = exoneracionApplication;
            _generateExcelApplication = generateExcelApplication;
        }

        [HttpGet]
        public async Task<IActionResult> ListExoneracion([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _exoneracionApplication.ListExoneracion(filters);

            /*if ((bool)filters.Download!)
            {
                var columnNames = ExcelColumnNames.GetColumnsProveedores();
                var fileBytes = _generateExcelApplication.GenerateToExcelGeneric(response.Data!, columnNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }*/

            return Ok(response);
        }

        [HttpGet("Cliente")]
        public async Task<IActionResult> ListExoneracionCliente([FromQuery] BaseFiltersRequest filters, string cliente)
        {
            var response = await _exoneracionApplication.ListExoneracionCliente(filters, cliente);

            /*if ((bool)filters.Download!)
            {
                var columnNames = ExcelColumnNames.GetColumnsProveedores();
                var fileBytes = _generateExcelApplication.GenerateToExcelGeneric(response.Data!, columnNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }*/

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ExoneracionById(int id)
        {
            var response = await _exoneracionApplication.ExoneracionById(id);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterExoneracion([FromForm] ExoneracionRequestDto requestDto)
        {
            var response = await _exoneracionApplication.RegisterExoneracion(requestDto);
            return Ok(response);
        }

        [HttpPut("Edit/{id:int}")]
        public async Task<IActionResult> EditExoneracion(int id, [FromForm] ExoneracionRequestDto requestDto)
        {
            var response = await _exoneracionApplication.EditExoneracion(id, requestDto);
            return Ok(response);
        }

        [HttpPut("Remove/{id:int}")]
        public async Task<IActionResult> RemoveExoneracion(int id)
        {
            var response = await _exoneracionApplication.RemoveExoneracion(id);

            return Ok(response);
        }
    }
}