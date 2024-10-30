using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts;

public partial class DbCfContext : DbContext
{
    public DbCfContext()
    {
    }

    public DbCfContext(DbContextOptions<DbCfContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbUsuario> TbUsuarios { get; set; }

    public virtual DbSet<TbItinerario> TbItinerarios { get; set; }

    public virtual DbSet<TbNoticia> TbNoticias { get; set; }

    public virtual DbSet<TbEmpleo> TbEmpleos { get; set; }

    public virtual DbSet<TbWhs> TbWhs { get; set; }

    public virtual DbSet<TbFinance> TbFinances { get; set; }

    public virtual DbSet<TbExoneracion> TbExoneracions { get; set; }

    public virtual DbSet<TbRol> TbRols { get; set; }

    public virtual DbSet<TbLogs> TbLogs { get; set; }

    public virtual DbSet<TbOrigen> TbOrigens { get; set; }

    public virtual DbSet<TbPol> TbPOLs { get; set; }

    public virtual DbSet<TbDestino> TbDestinos { get; set; }

    public virtual DbSet<TbPod> TbPODs { get; set; }

    public virtual DbSet<TbControlInventarioWhs> TbControlInventarioWhs { get; set; }

    public virtual DbSet<TbMultimedia> TbMultimedias { get; set; }

    public virtual DbSet<TbBcf> TbBcfs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("Relational.Collaction", "Modern_Spanish_CI_AS");

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
