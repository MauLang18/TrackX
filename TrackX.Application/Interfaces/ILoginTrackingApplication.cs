using TrackX.Application.Commons.Bases.Response;
using TrackX.Domain.Entities;

namespace TrackX.Application.Interfaces;

public interface ILoginTrackingApplication
{
    Task<BaseResponse<Dynamics<DynamicsLoginTracking>>> TrackingByIDTRA(string idtra, string cliente);
    Task<BaseResponse<Dynamics<DynamicsLoginTracking>>> TrackingByPO(string po, string cliente);
    Task<BaseResponse<Dynamics<DynamicsLoginTracking>>> TrackingByBCF(string bcf, string cliente);
    Task<BaseResponse<Dynamics<DynamicsLoginTracking>>> TrackingByContenedor(string contenedor, string cliente);
    Task<BaseResponse<Dynamics<DynamicsLoginTracking>>> TrackingByBooking(string booking, string cliente);
}