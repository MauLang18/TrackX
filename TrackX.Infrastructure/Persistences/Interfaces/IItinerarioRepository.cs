using TrackX.Domain.Entities;
using TrackX.Infrastructure.Commons.Bases.Request;
using TrackX.Infrastructure.Commons.Bases.Response;

namespace TrackX.Infrastructure.Persistences.Interfaces
{
    public interface IItinerarioRepository : IGenericRepository<TbItinerario>
    {
        Task<BaseEntityResponse<TbItinerario>> ListItinerarios(BaseFiltersRequest filters);
    }
}