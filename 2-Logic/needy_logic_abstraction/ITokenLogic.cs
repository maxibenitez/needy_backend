using needy_dto;

namespace needy_logic_abstraction
{
    public interface ITokenLogic
    {
        Task<string> GenerateJwtToken(UserData user);

        Task<string> GetUserCIFromToken();
    }
}
