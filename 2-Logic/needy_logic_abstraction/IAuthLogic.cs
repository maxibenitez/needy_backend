using needy_dto;
using needy_logic_abstraction.Parameters;

namespace needy_logic_abstraction
{
    public interface IAuthLogic
    {
        Task<string> LoginAsync(LoginParameters parameters);

        Task<bool> RegisterAsync(RegisterParameters parameters);
    }
}