using TrackX.Application.Commons.Bases.Response;
using TrackX.Domain.Entities;

namespace TrackX.Application.Interfaces
{
    public interface IClienteApplication
    {
        Task<BaseResponse<DynamicsClientes>> NombreCliente(string code);
    }
}