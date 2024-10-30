using TrackX.Application.Commons.Bases.Response;
using TrackX.Domain.Entities;

namespace TrackX.Application.Interfaces;

public interface IClienteApplication
{
    Task<BaseResponse<Dynamics<DynamicsClientes>>> NombreCliente(List<string> code);
    Task<BaseResponse<Dynamics<DynamicsClientes>>> CodeCliente(string name);
}