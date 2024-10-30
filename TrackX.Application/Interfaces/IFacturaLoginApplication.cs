using TrackX.Application.Commons.Bases.Response;
using TrackX.Domain.Entities;

namespace TrackX.Application.Interfaces;

public interface IFacturaLoginApplication
{
    Task<BaseResponse<Dynamics<DynamicsFacturas>>> TrackingByFactura(string factura, string cliente);
}