using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Interfaces
{
    public interface IRolPrivilegioRepository
    {
        Task<bool> RegisterRolPrivilegio(TbRolPrivilegio rolPrivilegio);
        Task<IEnumerable<TbRolPrivilegio>> GetRolPrivilegioByRol(int idRol);
        Task<TbRolPrivilegio> GetRolPrivilegioById(int id);
        Task<bool> UpdateRolPrivilegio(TbRolPrivilegio rolPrivilegio);
    }
}