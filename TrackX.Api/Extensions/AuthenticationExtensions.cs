using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Secret;

namespace TrackX.Api.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceProvider = services.BuildServiceProvider();
            var secretService = serviceProvider.GetRequiredService<ISecretService>();

            var secretJson = secretService.GetSecret("TrackX/data/Jwt").GetAwaiter().GetResult();
            var SecretResponse = JsonConvert.DeserializeObject<SecretResponse<JwtConfig>>(secretJson);
            var Config = SecretResponse?.Data?.Data;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Config!.Issuer,
                        ValidAudience = Config.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.Secret!))
                    };
                });

            return services;
        }
    }
}
