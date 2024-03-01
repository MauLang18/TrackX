using TrackX.Application.Commons.Bases;
using TrackX.Domain.Entities;

namespace TrackX.Application.Interfaces
{
    public interface ITrackingNoLoginApplication
    {
        Task<BaseResponse<Dynamics>> TrackingByIDTRA(string idtra);
        Task<BaseResponse<Dynamics>> TrackingByPO(string po);
        Task<BaseResponse<Dynamics>> TrackingByBCF(string bcf);
        Task<BaseResponse<Dynamics>> TrackingByContenedor(string contenedor);
    }
}