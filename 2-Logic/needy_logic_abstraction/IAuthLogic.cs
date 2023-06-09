using needy_dto;
using needy_logic_abstraction.Enumerables;
using needy_logic_abstraction.Parameters;

namespace needy_logic_abstraction
{
    public interface IAuthLogic
    {
        Task<Session> LoginAsync(LoginParameters parameters);

        Task<ErrorStatus> RegisterAsync(RegisterParameters parameters);
    }
}