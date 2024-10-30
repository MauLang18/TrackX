using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Interfaces;

public interface IBcfRepository : IGenericRepository<TbBcf>
{
    Task<string?> GetLastBcfAsync();
}