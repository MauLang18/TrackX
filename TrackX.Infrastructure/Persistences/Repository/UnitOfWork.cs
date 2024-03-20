using Microsoft.Extensions.Configuration;
using TrackX.Infrastructure.FileStorage;
using TrackX.Infrastructure.Persistences.Contexts;
using TrackX.Infrastructure.Persistences.Interfaces;

namespace TrackX.Infrastructure.Persistences.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbCfContext _context;

        public IUsuarioRepository Usuario { get; private set; }

        public IRolRepository Rol {  get; private set; }

        public IAzureStorage Storage { get; private set; }

        public IEmpleoRepository Empleo { get; private set; }

        public IItinerarioRepository Itinerario { get; private set; }

        public INoticiaRepository Noticia { get; private set; }

        public UnitOfWork(DbCfContext context, IConfiguration configuration)
        {
            _context = context;
            Usuario = new UsuarioRepository(_context);
            Rol = new RolRepository(_context);
            Storage = new AzureStorage(configuration);
            Empleo = new EmpleoRepository(_context);
            Itinerario = new ItinerarioRepository(_context);
            Noticia = new NoticiaRepository(_context);
        }

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