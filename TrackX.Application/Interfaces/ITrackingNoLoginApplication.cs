using TrackX.Application.Commons.Bases;
using TrackX.Domain.Entities;

namespace TrackX.Application.Interfaces
{
    public interface ITrackingNoLoginApplication
    {
        Task<BaseResponse<Dynamics>> TrackingByIDTRA(string idtra);
        Task<BaseResponse<string>> TrackingByPO(string po);
        Task<BaseResponse<string>> TrackingByBCF(string bcf);
        Task<BaseResponse<string>> TrackingByContenedor(string contenedor);
    }
}