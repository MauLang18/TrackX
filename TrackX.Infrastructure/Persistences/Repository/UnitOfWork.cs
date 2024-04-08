using Microsoft.Extensions.Configuration;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Contexts;
using TrackX.Infrastructure.Persistences.Interfaces;

namespace TrackX.Infrastructure.Persistences.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbCfContext _context;
        private IGenericRepository<TbRol> _rol = null!;
        private IUsuarioRepository _usuario = null!;
        private IGenericRepository<TbEmpleo> _empleo = null!;
        private IGenericRepository<TbItinerario> _itinerario = null!;
        private IGenericRepository<TbNoticia> _noticia = null!;
        private IGenericRepository<TbWhs> _whs = null!;
        private IGenericRepository<TbFinance> _finance = null!;
        private IGenericRepository<TbExoneracion> _exoneracion = null!;

        public UnitOfWork(DbCfContext context, IConfiguration configuration)
        {
            _context = context;
        }

        public IUsuarioRepository Usuario => _usuario ?? new UsuarioRepository(_context);

        public IGenericRepository<TbRol> Rol => _rol ?? new GenericRepository<TbRol>(_context);

        public IGenericRepository<TbEmpleo> Empleo => _empleo ?? new GenericRepository<TbEmpleo>(_context);

        public IGenericRepository<TbItinerario> Itinerario => _itinerario ?? new GenericRepository<TbItinerario>(_context);

        public IGenericRepository<TbNoticia> Noticia => _noticia ?? new GenericRepository<TbNoticia>(_context);

        public IGenericRepository<TbWhs> Whs => _whs ?? new GenericRepository<TbWhs>(_context);

        public IGenericRepository<TbFinance> Finance => _finance ?? new GenericRepository<TbFinance>(_context);

        public IGenericRepository<TbExoneracion> Exoneracion => _exoneracion ?? new GenericRepository<TbExoneracion>(_context);

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}