using Microsoft.Extensions.DependencyInjection;
using needy_logic;
using needy_logic_abstraction;

namespace needy_logic_dependency_injection
{
    public static class Extensions
    {
        public static IServiceCollection AddLogic(this IServiceCollection services)
        {
            return services
            .AddScoped<IUserLogic, UserLogic>()
            .AddScoped<IAuthLogic, AuthLogic>()
            .AddScoped<INeedLogic, NeedLogic>()
            .AddScoped<IRatingLogic, RatingLogic>()
            .AddSingleton<IUserContext, UserContextLogic>();
        }
    }
}