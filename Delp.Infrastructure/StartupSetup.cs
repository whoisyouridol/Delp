using Delp.Application.InfrastructureAbstractions;
using Delp.Application.Repositories;
using Delp.Infrastructure.Implementations;
using Delp.Infrastructure.Persistence;
using Delp.Infrastructure.RepositoriesImplementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Delp.Infrastructure
{
    public static class StartupSetup
    {
        public static void AddApplicationDbContext(this IServiceCollection services, string connectionString)
            => services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IMailService, MailService>();
        }
    }
}