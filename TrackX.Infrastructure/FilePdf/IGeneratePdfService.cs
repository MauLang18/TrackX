using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.FilePdf;

public interface IGeneratePdfService
{
    MemoryStream GeneratePdf(Dynamics<DynamicsTransInternacional> data);
}