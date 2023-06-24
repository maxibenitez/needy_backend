using needy_dto;
using needy_logic_abstraction.Parameters;

namespace needy_logic_abstraction
{
    public interface IUserLogic
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<IEnumerable<User>> GetUsersBySkillAsync(int skillId);

        Task<User> GetUserByCIAsync(string userCI);

        Task<IEnumerable<User>> GetUsersBySkillNameAsync(string skillName);

        Task<bool> UpdateUserAsync(UpdateUserParameters parameters);
    }
}
