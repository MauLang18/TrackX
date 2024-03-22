using TrackX.Application.Commons.Bases.Response;
using TrackX.Domain.Entities;

namespace TrackX.Application.Interfaces
{
    public interface ILoginTrackingApplication
    {
        Task<BaseResponse<DynamicsLoginTracking>> TrackingByIDTRA(string idtra);
        Task<BaseResponse<DynamicsLoginTracking>> TrackingByPO(string po);
        Task<BaseResponse<DynamicsLoginTracking>> TrackingByBCF(string bcf);
        Task<BaseResponse<DynamicsLoginTracking>> TrackingByContenedor(string contenedor);
    }
}