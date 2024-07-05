using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Dtos.Multimedia.Request;
using TrackX.Application.Interfaces;
using TrackX.Utilities.Static;

namespace TrackX.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MultimediaController : ControllerBase
{
    private readonly IMultimediaApplication _multimediaApplication;
    private readonly IGenerateExcelApplication _generateExcelApplication;

    public MultimediaController(IMultimediaApplication multimediaApplication, IGenerateExcelApplication generateExcelApplication)
    {
        _multimediaApplication = multimediaApplication;
        _generateExcelApplication = generateExcelApplication;
    }

    [HttpGet]
    public async Task<IActionResult> ListMultimedia(BaseFiltersRequest filters)
    {
        var response = await _multimediaApplication.ListMultimedia(filters);

        if ((bool)filters.Download!)
        {
            var columnNames = ExcelColumnNames.GetColumnsMultimedia();
            var fileBytes = _generateExcelApplication.GenerateToExcelGeneric(response.Data!, columnNames);
            return File(fileBytes, ContentType.ContentTypeExcel);
        }

        return Ok(response);
    }

    [HttpGet("Select")]
    public async Task<IActionResult> ListMultimediaSelect()
    {
        var response = await _multimediaApplication.ListMultimediaSelect();

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> MultimediaById(int id)
    {
        var response = await _multimediaApplication.MultimediaById(id);

        return Ok(response);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterMultimedia([FromForm] MultimediaRequestDto request)
    {
        var response = await _multimediaApplication.RegisterMultimedia(request);

        return Ok(response);
    }

    [HttpPut("Edit/{id:int}")]
    public async Task<IActionResult> EditMultimedia(int id, [FromForm] MultimediaRequestDto request)
    {
        var response = await _multimediaApplication.EditMultimedia(id, request);

        return Ok(response);
    }

    [HttpPut("Remove/{id:int}")]
    public async Task<IActionResult> RemoveMultimedia(int id)
    {
        var response = await _multimediaApplication.RemoveMultimedia(id);

        return Ok(response);
    }
}