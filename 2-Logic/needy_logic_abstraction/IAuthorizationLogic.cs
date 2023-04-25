using needy_dto;
using needy_logic_abstraction.Parameters;

namespace needy_logic_abstraction
{
    public interface IAuthorizationLogic
    {
        Task<User> AuthenticateAsync(AuthenticationParameters parameters);
    }
}