using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Dtos.Pol.Request;
using TrackX.Application.Interfaces;
using TrackX.Utilities.Static;

namespace TrackX.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PolController : ControllerBase
{
    private readonly IPolApplication _polApplication;
    private readonly IGenerateExcelApplication _generateExcelApplication;

    public PolController(IPolApplication polApplication, IGenerateExcelApplication generateExcelApplication)
    {
        _polApplication = polApplication;
        _generateExcelApplication = generateExcelApplication;
    }

    [HttpGet]
    public async Task<IActionResult> ListPoles([FromQuery] BaseFiltersRequest filters)
    {
        var response = await _polApplication.ListPol(filters);

        if ((bool)filters.Download!)
        {
            var columnNames = ExcelColumnNames.GetColumnsPol();
            var fileBytes = _generateExcelApplication.GenerateToExcelGeneric(response.Data!, columnNames);
            return File(fileBytes, ContentType.ContentTypeExcel);
        }

        return Ok(response);
    }

    [HttpGet("Select")]
    public async Task<IActionResult> ListSelectPoles()
    {
        var response = await _polApplication.ListSelectPol();
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> PolById(int id)
    {
        var response = await _polApplication.PolById(id);

        return Ok(response);
    }

    [HttpGet("Whs")]
    public async Task<IActionResult> PolByWhs()
    {
        var response = await _polApplication.ListSelectPolWhs();

        return Ok(response);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterPol([FromBody] PolRequestDto requestDto)
    {
        var response = await _polApplication.RegisterPol(requestDto);

        return Ok(response);
    }

    [HttpPut("Edit/{id:int}")]
    public async Task<IActionResult> EditRol(int id, [FromBody] PolRequestDto requestDto)
    {
        var response = await _polApplication.EditPol(id, requestDto);
        return Ok(response);
    }

    [HttpPut("Remove/{id:int}")]
    public async Task<IActionResult> RemovePol(int id)
    {
        var response = await _polApplication.RemovePol(id);

        return Ok(response);
    }
}