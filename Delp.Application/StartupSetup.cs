using Delp.Application.Abstractions;
using Delp.Application.ConfigurationModels;
using Delp.Application.Implementations;
using Delp.Application.InfrastructureAbstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace Delp.Application;
public static class StartupSetup
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtFactory, JwtFactory>();

        services.AddSingleton(provider => provider.GetRequiredService<IOptions<EmailConfiguration>>().Value);
    }
}