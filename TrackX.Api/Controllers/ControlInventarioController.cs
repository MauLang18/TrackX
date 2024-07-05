using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Dtos.ControlInventario.Request;
using TrackX.Application.Interfaces;
using TrackX.Utilities.Static;

namespace TrackX.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ControlInventarioController : ControllerBase
{
    private readonly IControlInventarioApplication _ControlInventarioApplication;
    private readonly IGenerateExcelApplication _generateExcelApplication;

    public ControlInventarioController(IControlInventarioApplication controlInventarioApplication, IGenerateExcelApplication generateExcelApplication)
    {
        _ControlInventarioApplication = controlInventarioApplication;
        _generateExcelApplication = generateExcelApplication;
    }

    [HttpGet]
    public async Task<IActionResult> ListControlInventario([FromQuery] BaseFiltersRequest filters, string whs)
    {
        var response = await _ControlInventarioApplication.ListControlInventario(filters, whs);

        if ((bool)filters.Download!)
        {
            var columnNames = ExcelColumnNames.GetColumnsControlInventario();
            var fileBytes = _generateExcelApplication.GenerateToExcelGeneric(response.Data!, columnNames);
            return File(fileBytes, ContentType.ContentTypeExcel);
        }

        return Ok(response);
    }

    [HttpGet("Cliente")]
    public async Task<IActionResult> ListControlInventarioCliente([FromQuery] BaseFiltersRequest filters, string cliente, string whs)
    {
        var response = await _ControlInventarioApplication.ListControlInventarioCliente(filters, cliente, whs);

        if ((bool)filters.Download!)
        {
            var columnNames = ExcelColumnNames.GetColumnsControlInventario();
            var fileBytes = _generateExcelApplication.GenerateToExcelGeneric(response.Data!, columnNames);
            return File(fileBytes, ContentType.ContentTypeExcel);
        }

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ControlInventarioById(int id)
    {
        var response = await _ControlInventarioApplication.ControlInventarioById(id);
        return Ok(response);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterControlInventario([FromForm] ControlInventarioRequestDto requestDto)
    {
        var response = await _ControlInventarioApplication.RegisterControlInventario(requestDto);
        return Ok(response);
    }

    [HttpPut("Edit/{id:int}")]
    public async Task<IActionResult> EditControlInventario(int id, [FromForm] ControlInventarioRequestDto requestDto)
    {
        var response = await _ControlInventarioApplication.EditControlInventario(id, requestDto);

        return Ok(response);
    }

    [HttpPut("Remove/{id:int}")]
    public async Task<IActionResult> RemoveControlInventario(int id)
    {
        var response = await _ControlInventarioApplication.RemoveControlInventario(id);

        return Ok(response);
    }
}