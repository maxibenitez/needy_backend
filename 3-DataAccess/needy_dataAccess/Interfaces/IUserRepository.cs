using needy_dto;
using needy_logic_abstraction.Parameters;

namespace needy_dataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserData>> GetUsersAsync(string userCI);

        Task<IEnumerable<UserData>> GetUsersBySkillAsync(int skillId);

        Task<UserData> GetUserByCIAsync(string userCI);

        Task<IEnumerable<UserData>> GetUsersBySkillNameAsync(string skillName);

        Task<UserData> GetUserByEmailAsync(string email);

        Task<bool> InsertUserAsync(RegisterParameters parameters);

        Task<bool> InsertUserSkillAsync(string userCI, int skillId);

        Task<bool> UpdateUserAsync(string userCI, UpdateUserParameters parameters);

        Task<bool> DeleteUserSkillsAsync(string userCI);
    }
}
