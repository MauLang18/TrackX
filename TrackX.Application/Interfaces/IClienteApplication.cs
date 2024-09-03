using TrackX.Application.Commons.Bases.Response;
using TrackX.Domain.Entities;

namespace TrackX.Application.Interfaces;

public interface IClienteApplication
{
    Task<BaseResponse<DynamicsClientes>> NombreCliente(List<string> code);
    Task<BaseResponse<DynamicsClientes>> CodeCliente(string name);
}