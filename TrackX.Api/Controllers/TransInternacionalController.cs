using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Dtos.TransInternacional.Request;
using TrackX.Application.Interfaces;
using TrackX.Infrastructure.FilePdf;
using TrackX.Utilities.Static;

namespace TrackX.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransInternacionalController : ControllerBase
{
    private readonly ITransInternacionalApplication _transInternacionalApplication;
    private readonly IGenerateExcelApplication _generateExcelApplication;
    private readonly IGeneratePdfApplication _generatePdfApplication;

    public TransInternacionalController(ITransInternacionalApplication transInternacionalApplication, IGenerateExcelApplication generateExcelApplication, IGeneratePdfApplication generatePdfApplication)
    {
        _transInternacionalApplication = transInternacionalApplication;
        _generateExcelApplication = generateExcelApplication;
        _generatePdfApplication = generatePdfApplication;
    }

    [HttpGet]
    public async Task<IActionResult> ListTransInternacional(int numFilter = 0, string textFilter = null!)
    {
        var response = await _transInternacionalApplication.ListTransInternacional(numFilter, textFilter);

        return Ok(response);
    }

    [HttpGet("Pdf")]
    public async Task<IActionResult> ListTransInternacionalPdf(string textFilter = null!)
    {
        var response = await _transInternacionalApplication.ListTransInternacional(6, textFilter);

        // Llamar al método GenerateToPdf para generar el archivo PDF
        var fileBytes = _generatePdfApplication.GenerateToPdf(response.Data!);

        // Verificar que los datos del archivo no estén vacíos
        if (fileBytes.Length == 0)
        {
            return BadRequest("Error al generar el PDF.");
        }

        // Devolver el archivo PDF con el tipo de contenido adecuado
        return File(fileBytes, "application/pdf", "Documento.pdf");
    }

    [HttpPatch("Agregar")]
    public async Task<IActionResult> RegisterComentario([FromBody] TransInternacionalRequestDto request)
    {
        var response = await _transInternacionalApplication.RegisterComentario(request);

        return Ok(response);
    }

    [HttpPatch("Upload")]
    public async Task<IActionResult> UpdateDocuments([FromForm] TransInternacionalDocumentRequestDto request)
    {
        var response = await _transInternacionalApplication.UpdateDocuments(request);

        return Ok(response);
    }

    [HttpGet("Download")]
    public async Task<IActionResult> DownloadActivo(int numFilter = 0, string textFilter = null!)
    {
        var response = await _transInternacionalApplication.ListTransInternacional(numFilter, textFilter);

        var columnNames = ExcelColumnNames.GetColumnsTrasporteInternacional();
        var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!.value!, columnNames);

        return File(fileBytes, ContentType.ContentTypeExcel);
    }
}