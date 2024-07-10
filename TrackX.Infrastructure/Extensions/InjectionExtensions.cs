using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrackX.Infrastructure.FileExcel;
using TrackX.Infrastructure.FileStorage;
using TrackX.Infrastructure.Persistences.Contexts;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Infrastructure.Persistences.Repository;

namespace TrackX.Infrastructure.Extensions;

public static class InjectionExtensions
{
    public static IServiceCollection AddInjectionInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(DbCfContext).Assembly.FullName;

        services.AddDbContext<DbCfContext>(
            options => options.UseSqlServer(
                   configuration.GetConnectionString("Connection"), b => b.MigrationsAssembly(assembly)), ServiceLifetime.Transient);

        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddTransient<IFileStorageLocal, FileStorageLocal>();
        services.AddTransient<IGenerateExcel, GenerateExcel>();
        services.AddTransient<IImportExcel, ImportExcel>();

        services.AddStackExchangeRedisCache(options =>
        {
            var redis = configuration.GetConnectionString("Redis");

            options.Configuration = redis;
        });

        return services;
    }
}