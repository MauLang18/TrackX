using TrackX.Domain.Entities;

namespace TrackX.Application.Interfaces;

public interface IGeneratePdfApplication
{
    public byte[] GenerateToPdf(Dynamics<DynamicsTransInternacional> data);
}