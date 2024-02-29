using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TrackX.Application.Extensions.WatchDog;
using TrackX.Application.Interfaces;
using TrackX.Application.Services;

namespace TrackX.Application.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IUsuarioApplication, UsuarioApplication>();
            services.AddScoped<IAuthApplication, AuthApplication>();
            services.AddScoped<ITrackingNoLoginApplication, TrackingNoLoginApplication>();
            services.AddScoped<IGenerateExcelApplication, GenerateExcelApplication>();

            services.AddWatchDog();

            return services;
        }
    }
}