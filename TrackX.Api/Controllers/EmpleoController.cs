using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Dtos.Empleo.Request;
using TrackX.Application.Interfaces;
using TrackX.Utilities.Static;

namespace TrackX.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmpleoController : ControllerBase
{
    private readonly IEmpleoApplication _empleoApplication;
    private readonly IGenerateExcelApplication _generateExcelApplication;

    public EmpleoController(IEmpleoApplication empleoApplication, IGenerateExcelApplication generateExcelApplication)
    {
        _empleoApplication = empleoApplication;
        _generateExcelApplication = generateExcelApplication;
    }

    [HttpGet]
    public async Task<IActionResult> ListEmpleos([FromQuery] BaseFiltersRequest filters)
    {
        var response = await _empleoApplication.ListEmpleos(filters);

        if ((bool)filters.Download!)
        {
            var columnNames = ExcelColumnNames.GetColumnsEmpleos();
            var fileBytes = _generateExcelApplication.GenerateToExcelGeneric(response.Data!, columnNames);
            return File(fileBytes, ContentType.ContentTypeExcel);
        }

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> EmpleoById(int id)
    {
        var response = await _empleoApplication.EmpleoById(id);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterEmpleo([FromForm] EmpleoRequestDto requestDto)
    {
        var response = await _empleoApplication.RegisterEmpleo(requestDto);
        return Ok(response);
    }

    [HttpPut("Edit/{id:int}")]
    public async Task<IActionResult> EditEmpleo(int id, [FromForm] EmpleoRequestDto requestDto)
    {
        var response = await _empleoApplication.EditEmpleo(id, requestDto);

        return Ok(response);
    }

    [HttpPut("Remove/{id:int}")]
    public async Task<IActionResult> RemoveEmpleo(int id)
    {
        var response = await _empleoApplication.RemoveEmpleo(id);

        return Ok(response);
    }
}