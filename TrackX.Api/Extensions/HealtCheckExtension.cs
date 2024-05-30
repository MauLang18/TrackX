using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TrackX.Api.Extensions
{
    public static class HealtCheckExtension
    {
        public static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services
            .AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("Connection")!, tags: new[] { "sqlserver" })
            .AddRedis(configuration.GetConnectionString("Redis")!, tags: new[] { "redis" });

            services
                .AddHealthChecksUI()
                .AddInMemoryStorage();

            return services;
        }
    }
}