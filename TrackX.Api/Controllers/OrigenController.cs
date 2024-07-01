using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Dtos.Origen.Request;
using TrackX.Application.Interfaces;
using TrackX.Utilities.Static;

namespace TrackX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrigenController : ControllerBase
    {
        private readonly IOrigenApplication _origenApplication;
        private readonly IGenerateExcelApplication _generateExcelApplication;

        public OrigenController(IOrigenApplication origenApplication, IGenerateExcelApplication generateExcelApplication)
        {
            _origenApplication = origenApplication;
            _generateExcelApplication = generateExcelApplication;
        }

        [HttpGet]
        public async Task<IActionResult> ListOrigenes([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _origenApplication.ListOrigenes(filters);

            if ((bool)filters.Download!)
            {
                var columnNames = ExcelColumnNames.GetColumnsOrigen();
                var fileBytes = _generateExcelApplication.GenerateToExcelGeneric(response.Data!, columnNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }

            return Ok(response);
        }

        [HttpGet("Select")]
        public async Task<IActionResult> ListSelectOrigenes()
        {
            var response = await _origenApplication.ListSelectOrigen();
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> OrigenById(int id)
        {
            var response = await _origenApplication.OrigenById(id);

            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterOrigen([FromForm] OrigenRequestDto requestDto)
        {
            var response = await _origenApplication.RegisterOrigen(requestDto);

            return Ok(response);
        }

        [HttpPut("Edit/{id:int}")]
        public async Task<IActionResult> EditRol(int id, [FromBody] OrigenRequestDto requestDto)
        {
            var response = await _origenApplication.EditOrigen(id, requestDto);
            return Ok(response);
        }

        [HttpPut("Remove/{id:int}")]
        public async Task<IActionResult> RemoveOrigen(int id)
        {
            var response = await _origenApplication.RemoveOrigen(id);

            return Ok(response);
        }
    }
}