using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Dtos.Logs.Request;
using TrackX.Application.Interfaces;
using TrackX.Utilities.Static;

namespace TrackX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogsApplication _logsApplication;
        private readonly IGenerateExcelApplication _generateExcelApplication;

        public LogsController(ILogsApplication logsApplication, IGenerateExcelApplication generateExcelApplication)
        {
            _logsApplication = logsApplication;
            _generateExcelApplication = generateExcelApplication;
        }

        [HttpGet]
        public async Task<IActionResult> ListLogs([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _logsApplication.ListLogs(filters);

            if ((bool)filters.Download!)
            {
                var columnNames = ExcelColumnNames.GetColumnsItinerarios();
                var fileBytes = _generateExcelApplication.GenerateToExcelGeneric(response.Data!, columnNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> LogById(int id)
        {
            var response = await _logsApplication.LogById(id);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterLog([FromBody] LogsRequestDto requestDto)
        {
            var response = await _logsApplication.RegisterLog(requestDto);
            return Ok(response);
        }

        [HttpPut("Remove/{id:int}")]
        public async Task<IActionResult> RemoveLog(int id)
        {
            var response = await _logsApplication.RemoveLog(id);

            return Ok(response);
        }
    }
}
