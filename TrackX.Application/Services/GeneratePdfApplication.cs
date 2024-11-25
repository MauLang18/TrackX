using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.FilePdf;

namespace TrackX.Application.Services;

public class GeneratePdfApplication : IGeneratePdfApplication
{
    private readonly IGeneratePdfService _generatePdfService;

    public GeneratePdfApplication(IGeneratePdfService generatePdfService)
    {
        _generatePdfService = generatePdfService;
    }

    public byte[] GenerateToPdf(Dynamics<DynamicsTransInternacional> data)
    {
        // Llamamos al servicio de generación de PDF para obtener el MemoryStream
        var pdfMemoryStream = _generatePdfService.GeneratePdf(data);

        // Convertimos el MemoryStream a byte[]
        var fileBytes = pdfMemoryStream.ToArray();

        return fileBytes;
    }
}