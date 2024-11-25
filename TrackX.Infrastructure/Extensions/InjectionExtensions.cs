using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.FileExcel;
using TrackX.Infrastructure.FilePdf;
using TrackX.Infrastructure.FileStorage;
using TrackX.Infrastructure.Persistences.Contexts;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Infrastructure.Persistences.Repository;
using TrackX.Infrastructure.Secret;

namespace TrackX.Infrastructure.Extensions;

public static class InjectionExtensions
{
    public static IServiceCollection AddInjectionInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceProvider = services.BuildServiceProvider();
        var secretService = serviceProvider.GetRequiredService<ISecretService>();

        var secretJson = secretService.GetSecret("TrackX/data/ConnectionStrings").GetAwaiter().GetResult();
        var SecretResponse = JsonConvert.DeserializeObject<SecretResponse<ConnectionStringsConfig>>(secretJson);
        var Config = SecretResponse?.Data?.Data;

        var assembly = typeof(DbCfContext).Assembly.FullName;

        services.AddDbContext<DbCfContext>(
            options => options.UseSqlServer(
                   Config!.Connection, b => b.MigrationsAssembly(assembly)), ServiceLifetime.Transient);

        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddTransient<IFileStorageLocal, FileStorageLocal>();
        services.AddTransient<IGenerateExcel, GenerateExcel>();
        services.AddTransient<IImportExcel, ImportExcel>();
        services.AddTransient<IGeneratePdfService, GeneratePdfService>();

        services.AddStackExchangeRedisCache(options =>
        {
            var redis = Config!.Redis;

            options.Configuration = redis;
        });

        return services;
    }
}