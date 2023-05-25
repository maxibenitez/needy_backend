using needy_dto;

namespace needy_logic_abstraction
{
    public interface IUserLogic
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<IEnumerable<User>> GetUsersBySkillAsync(int skillId);

        Task<User> GetUserByCIAsync(string userCI);

        Task<bool> InsertUserSkillAsync(int skilId);
    }
}
