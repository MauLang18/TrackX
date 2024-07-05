using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Dtos.Destino.Request;
using TrackX.Application.Interfaces;
using TrackX.Utilities.Static;

namespace TrackX.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DestinoController : ControllerBase
{
    private readonly IDestinoApplication _destinoApplication;
    private readonly IGenerateExcelApplication _generateExcelApplication;

    public DestinoController(IDestinoApplication destinoApplication, IGenerateExcelApplication generateExcelApplication)
    {
        _destinoApplication = destinoApplication;
        _generateExcelApplication = generateExcelApplication;
    }

    [HttpGet]
    public async Task<IActionResult> ListDestinos([FromQuery] BaseFiltersRequest filters)
    {
        var response = await _destinoApplication.ListDestinos(filters);

        if ((bool)filters.Download!)
        {
            var columnNames = ExcelColumnNames.GetColumnsDestino();
            var fileBytes = _generateExcelApplication.GenerateToExcelGeneric(response.Data!, columnNames);
            return File(fileBytes, ContentType.ContentTypeExcel);
        }

        return Ok(response);
    }

    [HttpGet("Select")]
    public async Task<IActionResult> ListSelectDestinos()
    {
        var response = await _destinoApplication.ListSelectDestino();
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> DestinoById(int id)
    {
        var response = await _destinoApplication.DestinoById(id);

        return Ok(response);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterDestino([FromForm] DestinoRequestDto requestDto)
    {
        var response = await _destinoApplication.RegisterDestino(requestDto);

        return Ok(response);
    }

    [HttpPut("Edit/{id:int}")]
    public async Task<IActionResult> EditRol(int id, [FromBody] DestinoRequestDto requestDto)
    {
        var response = await _destinoApplication.EditDestino(id, requestDto);
        return Ok(response);
    }

    [HttpPut("Remove/{id:int}")]
    public async Task<IActionResult> RemoveDestino(int id)
    {
        var response = await _destinoApplication.RemoveDestino(id);

        return Ok(response);
    }
}