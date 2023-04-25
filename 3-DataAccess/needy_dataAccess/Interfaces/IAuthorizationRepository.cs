using needy_dto;
using needy_logic_abstraction.Parameters;

namespace needy_dataAccess.Interfaces
{
    public interface IAuthorizationRepository
    {
        Task<User> AuthenticateAsync(AuthenticationParameters parameters);
    }
}
