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

        public UnitOfWork(DbCfContext context, IConfiguration configuration)
        {
            _context = context;
            Usuario = new UsuarioRepository(_context);
            Rol = new RolRepository(_context);
            Storage = new AzureStorage(configuration);
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