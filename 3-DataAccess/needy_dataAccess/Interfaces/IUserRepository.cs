using needy_dto;
using needy_logic_abstraction.Parameters;

namespace needy_dataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserData>> GetUsersAsync();

        Task<IEnumerable<UserData>> GetUsersBySkillAsync(int skillId);

        Task<UserData> GetUserByCIAsync(string userCI);

        Task<UserData> GetUserByEmailAsync(string email);

        Task<IEnumerable<Skill>> GetUserSkillsAsync(string userCI);

        Task<bool> InsertUserAsync(RegisterParameters parameters);

        Task<bool> InsertUserSkillAsync(string userCI, int skillId);
    }
}
