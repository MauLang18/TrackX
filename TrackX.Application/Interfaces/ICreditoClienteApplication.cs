using TrackX.Application.Commons.Bases.Response;
using TrackX.Domain.Entities;

namespace TrackX.Application.Interfaces;

public interface ICreditoClienteApplication
{
    Task<BaseResponse<Dynamics<DynamicsCreditoCliente>>> CreditoCliente(string code);
}