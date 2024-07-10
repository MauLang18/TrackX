using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Contexts;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;

namespace TrackX.Infrastructure.Persistences.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly DbCfContext _context;
    private readonly DbSet<T> _entity;

    public GenericRepository(DbCfContext context)
    {
        _context = context;
        _entity = _context.Set<T>();
    }

    public IQueryable<T> GetAllQueryable()
    {
        var getAllQuery = GetEntityQuery(x => x.UsuarioEliminacionAuditoria == null && x.FechaEliminacionAuditoria == null);
        return getAllQuery;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var getAll = await _entity
            .Where(x => x.Estado.Equals((int)StateTypes.Activo) && x.UsuarioEliminacionAuditoria == null && x.FechaEliminacionAuditoria == null)
            .AsNoTracking()
            .ToListAsync();

        return getAll;
    }

    public async Task<IEnumerable<T>> GetSelectAsync()
    {
        var getAll = await _entity
            .Where(x => x.Estado.Equals((int)StateTypes.Activo) && x.UsuarioEliminacionAuditoria == null && x.FechaEliminacionAuditoria == null)
            .AsNoTracking()
            .ToListAsync();

        return getAll;
    }

    public async Task<T> GetByIdAsync(int id)
    {
        var getById = await _entity!
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        return getById!;
    }

    public async Task<bool> RegisterAsync(T entity)
    {
        entity.UsuarioCreacionAuditoria = 1;
        entity.FechaCreacionAuditoria = DateTime.Now;

        await _context.AddAsync(entity);

        var recordsAffected = await _context.SaveChangesAsync();

        return recordsAffected > 0;
    }

    public async Task<bool> RegisterRangeAsync(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            entity.UsuarioCreacionAuditoria = 1;
            entity.FechaCreacionAuditoria = DateTime.Now;

            await _context.AddAsync(entity);
        }

        var recordsAffected = await _context.SaveChangesAsync();

        return recordsAffected > 0;
    }


    public async Task<bool> EditAsync(T entity)
    {
        entity.UsuarioActualizacionAuditoria = 1;
        entity.FechaActualizacionAuditoria = DateTime.Now;

        _context.Update(entity);

        _context.Entry(entity).Property(x => x.UsuarioCreacionAuditoria).IsModified = false;
        _context.Entry(entity).Property(x => x.FechaCreacionAuditoria).IsModified = false;

        var recordsAffected = await _context.SaveChangesAsync();

        return recordsAffected > 0;
    }

    public async Task<bool> RemoveAsync(int id)
    {
        T entity = await GetByIdAsync(id);

        entity.UsuarioEliminacionAuditoria = 1;
        entity.FechaEliminacionAuditoria = DateTime.Now;
        entity.Estado = 0;

        _context.Update(entity);

        var recordsAffected = await _context.SaveChangesAsync();

        return recordsAffected > 0;
    }

    public IQueryable<T> GetEntityQuery(Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = _entity;

        if (filter != null) query = query.Where(filter);

        return query;
    }
}