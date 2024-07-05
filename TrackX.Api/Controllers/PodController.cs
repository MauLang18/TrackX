using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Dtos.Pod.Request;
using TrackX.Application.Interfaces;
using TrackX.Utilities.Static;

namespace TrackX.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PodController : ControllerBase
{
    private readonly IPodApplication _podApplication;
    private readonly IGenerateExcelApplication _generateExcelApplication;

    public PodController(IPodApplication podApplication, IGenerateExcelApplication generateExcelApplication)
    {
        _podApplication = podApplication;
        _generateExcelApplication = generateExcelApplication;
    }

    [HttpGet]
    public async Task<IActionResult> ListPodes([FromQuery] BaseFiltersRequest filters)
    {
        var response = await _podApplication.ListPod(filters);

        if ((bool)filters.Download!)
        {
            var columnNames = ExcelColumnNames.GetColumnsPod();
            var fileBytes = _generateExcelApplication.GenerateToExcelGeneric(response.Data!, columnNames);
            return File(fileBytes, ContentType.ContentTypeExcel);
        }

        return Ok(response);
    }

    [HttpGet("Select")]
    public async Task<IActionResult> ListSelectPodes()
    {
        var response = await _podApplication.ListSelectPod();
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> PodById(int id)
    {
        var response = await _podApplication.PodById(id);

        return Ok(response);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterPod([FromBody] PodRequestDto requestDto)
    {
        var response = await _podApplication.RegisterPod(requestDto);

        return Ok(response);
    }

    [HttpPut("Edit/{id:int}")]
    public async Task<IActionResult> EditRol(int id, [FromBody] PodRequestDto requestDto)
    {
        var response = await _podApplication.EditPod(id, requestDto);
        return Ok(response);
    }

    [HttpPut("Remove/{id:int}")]
    public async Task<IActionResult> RemovePod(int id)
    {
        var response = await _podApplication.RemovePod(id);

        return Ok(response);
    }
}