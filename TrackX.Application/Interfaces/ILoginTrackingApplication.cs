using TrackX.Application.Commons.Bases.Response;
using TrackX.Domain.Entities;

namespace TrackX.Application.Interfaces
{
    public interface ILoginTrackingApplication
    {
        Task<BaseResponse<DynamicsLoginTracking>> TrackingByIDTRA(string idtra, string cliente);
        Task<BaseResponse<DynamicsLoginTracking>> TrackingByPO(string po, string cliente);
        Task<BaseResponse<DynamicsLoginTracking>> TrackingByBCF(string bcf, string cliente);
        Task<BaseResponse<DynamicsLoginTracking>> TrackingByContenedor(string contenedor, string cliente);
    }
}