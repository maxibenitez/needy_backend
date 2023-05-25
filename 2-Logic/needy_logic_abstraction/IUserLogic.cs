using needy_dto;
using needy_logic_abstraction.Parameters;

namespace needy_logic_abstraction
{
    public interface IUserLogic
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserByCIAsync(string userCI);

        Task<bool> InsertUserSkillAsync(int skilId);
    }
}
