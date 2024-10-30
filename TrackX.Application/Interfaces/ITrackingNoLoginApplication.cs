using TrackX.Application.Commons.Bases.Response;
using TrackX.Domain.Entities;

namespace TrackX.Application.Interfaces;

public interface ITrackingNoLoginApplication
{
    Task<BaseResponse<Dynamics<DynamicsTrackingNoLogin>>> TrackingByIDTRA(string idtra);
    Task<BaseResponse<Dynamics<DynamicsTrackingNoLogin>>> TrackingByPO(string po);
    Task<BaseResponse<Dynamics<DynamicsTrackingNoLogin>>> TrackingByBCF(string bcf);
    Task<BaseResponse<Dynamics<DynamicsTrackingNoLogin>>> TrackingByContenedor(string contenedor);
}