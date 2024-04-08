using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Dtos.Finance.Request;
using TrackX.Application.Interfaces;
using TrackX.Application.Services;
using TrackX.Utilities.Static;

namespace TrackX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceController : ControllerBase
    {
        private readonly IFinanceApplication _financeApplication;
        private readonly IGenerateExcelApplication _generateExcelApplication;

        public FinanceController(IFinanceApplication financeApplication, IGenerateExcelApplication generateExcelApplication)
        {
            _financeApplication = financeApplication;
            _generateExcelApplication = generateExcelApplication;
        }

        [HttpGet]
        public async Task<IActionResult> ListFinance([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _financeApplication.ListFinance(filters);

            /*if ((bool)filters.Download!)
            {
                var columnNames = ExcelColumnNames.GetColumnsProveedores();
                var fileBytes = _generateExcelApplication.GenerateToExcelGeneric(response.Data!, columnNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }*/

            return Ok(response);
        }

        [HttpGet("Cliente")]
        public async Task<IActionResult> ListFinanceCliente([FromQuery] BaseFiltersRequest filters, string cliente)
        {
            var response = await _financeApplication.ListFinanceCliente(filters, cliente);

            /*if ((bool)filters.Download!)
            {
                var columnNames = ExcelColumnNames.GetColumnsProveedores();
                var fileBytes = _generateExcelApplication.GenerateToExcelGeneric(response.Data!, columnNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }*/

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> FinanceById(int id)
        {
            var response = await _financeApplication.FinanceById(id);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterFinance([FromForm] FinanceRequestDto requestDto)
        {
            var response = await _financeApplication.RegisterFinance(requestDto);
            return Ok(response);
        }

        [HttpPut("Edit/{id:int}")]
        public async Task<IActionResult> EditFinance(int id, [FromForm] FinanceRequestDto requestDto)
        {
            var response = await _financeApplication.EditFinance(id, requestDto);

            return Ok(response);
        }

        [HttpPut("Remove/{id:int}")]
        public async Task<IActionResult> RemoveFinance(int id)
        {
            var response = await _financeApplication.RemoveFinance(id);

            return Ok(response);
        }
    }
}