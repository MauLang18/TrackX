using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<TbUsuario>
    {
        Task<TbUsuario> UserByEmail(string email);
    }
}