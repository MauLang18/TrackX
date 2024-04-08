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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("Relational.Collaction", "Modern_Spanish_CI_AS");

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
