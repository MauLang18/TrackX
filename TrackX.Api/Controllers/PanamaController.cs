using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackX.Application.Dtos.Panama.Request;
using TrackX.Application.Interfaces;
using TrackX.Application.Services;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;

namespace TrackX.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PanamaController : ControllerBase
{
    private readonly IPanamaApplication _panamaApplication;
    private readonly IGenerateExcelApplication _generateExcelApplication;

    public PanamaController(IPanamaApplication panamaApplication, IGenerateExcelApplication generateExcelApplication)
    {
        _panamaApplication = panamaApplication;
        _generateExcelApplication = generateExcelApplication;
    }

    [HttpGet]
    public async Task<IActionResult> ListPanama(int numFilter = 0, string textFilter = null!, int type = 0)
    {
        var response = await _panamaApplication.ListPanama(numFilter, textFilter,type);

        return Ok(response);
    }

    [HttpPatch("Agregar")]
    public async Task<IActionResult> RegisterComentario([FromBody] PanamaRequestDto request)
    {
        var response = await _panamaApplication.RegisterComentario(request);

        return Ok(response);
    }

    [HttpPatch("Upload")]
    public async Task<IActionResult> UpdateDocuments([FromForm] PanamaDocumentRequestDto request)
    {
        var response = await _panamaApplication.UpdateDocuments(request);

        return Ok(response);
    }

    [HttpPatch("RemoveFile")]
    public async Task<IActionResult> RemoveDocuments([FromForm] PanamaRemoveDocumentRequestDto request)
    {
        var response = await _panamaApplication.RemoveDocuments(request);

        return Ok(response);
    }

    [HttpGet("Download")]
    public async Task<IActionResult> DownloadActivo(int numFilter = 0, string textFilter = null!, int type = 0)
    {
        var response = await _panamaApplication.ListPanama(numFilter, textFilter, type);

        var columnNames = type != 0
            ? ExcelColumnNames.GetColumnsPanamaReporte()
            : ExcelColumnNames.GetColumnsPanama();

        var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!.value!, columnNames);

        return File(fileBytes, ContentType.ContentTypeExcel);
    }
}