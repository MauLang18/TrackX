using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TrackX.Application.Commons.Ordering;
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

            services.AddTransient<IOrderingQuery, OrderingQuery>();
            services.AddTransient<IFileStorageLocalApplication, FileStorageLocalApplication>();

            services.AddScoped<IGenerateExcelApplication, GenerateExcelApplication>();

            services.AddScoped<IUsuarioApplication, UsuarioApplication>();
            services.AddScoped<IAuthApplication, AuthApplication>();
            services.AddScoped<IRolApplication, RolApplication>();
            services.AddScoped<IItinerarioApplication, ItinerarioApplication>();
            services.AddScoped<IEmpleoApplication, EmpleoApplication>();
            services.AddScoped<INoticiaApplication, NoticiaApplication>();
            services.AddScoped<ITrackingNoLoginApplication, TrackingNoLoginApplication>();
            services.AddScoped<ITrackingLoginApplication, TrackingLoginApplication>();
            services.AddScoped<IClienteApplication, ClienteApplication>();
            services.AddScoped<ILoginTrackingApplication, LoginTrackingApplication>();

            services.AddScoped<ISendEmailApplication, SendEmailApplication>();

            services.AddWatchDog();

            return services;
        }
    }
}