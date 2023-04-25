using Microsoft.Extensions.DependencyInjection;
using needy_dataAccess.Implementation;
using needy_dataAccess.Interfaces;
using needy_dataAccess.Repositories;

namespace needy_dataAccess
{
    public static class Extension
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            return services
            .AddSingleton<PostgreSQLConnection>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IAuthorizationRepository, AuthorizationRepository>()
            .AddScoped<IRaitingRepository, RaitingRepository>();
        }
    }
}