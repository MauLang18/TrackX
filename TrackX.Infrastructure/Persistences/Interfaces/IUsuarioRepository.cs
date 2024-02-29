using TrackX.Domain.Entities;
using TrackX.Infrastructure.Commons.Bases.Request;
using TrackX.Infrastructure.Commons.Bases.Response;

namespace TrackX.Infrastructure.Persistences.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<TbUsuario>
    {
        Task<BaseEntityResponse<TbUsuario>> ListUsuarios(BaseFiltersRequest filters);
        Task<TbUsuario> UserByEmail(string email);
    }
}