using needy_logic_abstraction.Enumerables;
using needy_logic_abstraction.Parameters;

namespace needy_logic_abstraction
{
    public interface IAuthLogic
    {
        Task<string> LoginAsync(LoginParameters parameters);

        Task<RegisterStatus> RegisterAsync(RegisterParameters parameters);
    }
}