using TrackX.Application.Commons.Bases.Response;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Commons.Bases.Response;

namespace TrackX.Application.Interfaces
{
    public interface ITrackingLoginApplication
    {
        Task<BaseResponse<DynamicsTrackingLogin>> TrackingActivoByCliente(string cliente);
        Task<BaseResponse<DynamicsTrackingLogin>> TrackingFinalizadoByCliente(string cliente);
    }
}