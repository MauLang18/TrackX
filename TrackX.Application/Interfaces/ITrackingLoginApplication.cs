using TrackX.Application.Commons.Bases.Response;
using TrackX.Domain.Entities;

namespace TrackX.Application.Interfaces;

public interface ITrackingLoginApplication
{
    Task<BaseResponse<Dynamics<DynamicsTrackingLogin>>> TrackingActivoByCliente(string cliente);
    Task<BaseResponse<Dynamics<DynamicsTrackingLogin>>> TrackingFinalizadoByCliente(string cliente);
    Task<BaseResponse<Dynamics<DynamicsTrackingLogin>>> TrackingHistorialByCliente(string cliente);
}