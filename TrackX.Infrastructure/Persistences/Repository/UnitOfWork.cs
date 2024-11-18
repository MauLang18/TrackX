using Microsoft.Extensions.Configuration;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Contexts;
using TrackX.Infrastructure.Persistences.Interfaces;

namespace TrackX.Infrastructure.Persistences.Repository;

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
    private IGenericRepository<TbLogs> _logs = null!;
    private IGenericRepository<TbOrigen> _origen = null!;
    private IGenericRepository<TbPol> _pol = null!;
    private IGenericRepository<TbDestino> _destino = null!;
    private IGenericRepository<TbPod> _pod = null!;
    private IGenericRepository<TbControlInventarioWhs> _controlInventario = null!;
    private IGenericRepository<TbMultimedia> _multimedia = null!;
    private IBcfRepository _bcf = null!;
    private IGenericRepository<TbCotizacion> _cotizacion = null!;

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

    public IGenericRepository<TbLogs> Logs => _logs ?? new GenericRepository<TbLogs>(_context);

    public IGenericRepository<TbOrigen> Origen => _origen ?? new GenericRepository<TbOrigen>(_context);

    public IGenericRepository<TbPol> Pol => _pol ?? new GenericRepository<TbPol>(_context);

    public IGenericRepository<TbDestino> Destino => _destino ?? new GenericRepository<TbDestino>(_context);

    public IGenericRepository<TbPod> Pod => _pod ?? new GenericRepository<TbPod>(_context);

    public IGenericRepository<TbControlInventarioWhs> ControlInventario => _controlInventario ?? new GenericRepository<TbControlInventarioWhs>(_context);

    public IGenericRepository<TbMultimedia> Multimedia => _multimedia ?? new GenericRepository<TbMultimedia>(_context);

    public IGenericRepository<TbCotizacion> Cotizacion => _cotizacion ?? new GenericRepository<TbCotizacion>(_context);

    public IBcfRepository Bcf => _bcf ?? new BcfRepository(_context);

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