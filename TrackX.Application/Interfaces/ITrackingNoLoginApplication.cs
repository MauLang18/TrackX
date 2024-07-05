using TrackX.Application.Commons.Bases.Response;
using TrackX.Domain.Entities;

namespace TrackX.Application.Interfaces;

public interface ITrackingNoLoginApplication
{
    Task<BaseResponse<DynamicsTrackingNoLogin>> TrackingByIDTRA(string idtra);
    Task<BaseResponse<DynamicsTrackingNoLogin>> TrackingByPO(string po);
    Task<BaseResponse<DynamicsTrackingNoLogin>> TrackingByBCF(string bcf);
    Task<BaseResponse<DynamicsTrackingNoLogin>> TrackingByContenedor(string contenedor);
}