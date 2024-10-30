using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Dtos.Bcf.Request;
using TrackX.Application.Interfaces;
using TrackX.Utilities.Static;

namespace TrackX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BcfController : ControllerBase
    {
        private readonly IBcfApplication _bcfApplication;
        private readonly IGenerateExcelApplication _generateExcelApplication;

        public BcfController(IBcfApplication bcfApplication, IGenerateExcelApplication generateExcelApplication)
        {
            _bcfApplication = bcfApplication;
            _generateExcelApplication = generateExcelApplication;
        }

        [HttpGet]
        public async Task<IActionResult> ListBcf([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _bcfApplication.ListBcf(filters);

            if ((bool)filters.Download!)
            {
                var columnNames = ExcelColumnNames.GetColumnsBcf();
                var fileBytes = _generateExcelApplication.GenerateToExcelGeneric(response.Data!, columnNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }

            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterBcf([FromBody] BcfRequestDto request)
        {
            var response = await _bcfApplication.RegisterBcf(request);

            return Ok(response);
        }
    }
}