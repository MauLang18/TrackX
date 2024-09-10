using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Dtos.TransInternacional.Request;
using TrackX.Application.Interfaces;
using TrackX.Utilities.Static;

namespace TrackX.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransInternacionalController : ControllerBase
{
    private readonly ITransInternacionalApplication _transInternacionalApplication;
    private readonly IGenerateExcelApplication _generateExcelApplication;

    public TransInternacionalController(ITransInternacionalApplication transInternacionalApplication, IGenerateExcelApplication generateExcelApplication)
    {
        _transInternacionalApplication = transInternacionalApplication;
        _generateExcelApplication = generateExcelApplication;
    }

    [HttpGet]
    public async Task<IActionResult> ListTransInternacional(int numFilter = 0, string textFilter = null!)
    {
        var response = await _transInternacionalApplication.ListTransInternacional(numFilter, textFilter);

        return Ok(response);
    }

    [HttpPatch("Agregar")]
    public async Task<IActionResult> RegisterComentario([FromBody] TransInternacionalRequestDto request)
    {
        var response = await _transInternacionalApplication.RegisterComentario(request);

        return Ok(response);
    }

    [HttpGet("Download")]
    public async Task<IActionResult> DownloadActivo(int numFilter = 0, string textFilter = null!)
    {
        var response = await _transInternacionalApplication.ListTransInternacional(numFilter, textFilter);

        var columnNames = ExcelColumnNames.GetColumnsTrasporteInternacional();
        var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!.Value!, columnNames);

        return File(fileBytes, ContentType.ContentTypeExcel);
    }
}