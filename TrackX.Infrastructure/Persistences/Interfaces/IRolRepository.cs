using TrackX.Domain.Entities;
using TrackX.Infrastructure.Commons.Bases.Request;
using TrackX.Infrastructure.Commons.Bases.Response;

namespace TrackX.Infrastructure.Persistences.Interfaces
{
    public interface IRolRepository : IGenericRepository<TbRol>
    {
        Task<BaseEntityResponse<TbRol>> ListRoles(BaseFiltersRequest filters);
    }
}