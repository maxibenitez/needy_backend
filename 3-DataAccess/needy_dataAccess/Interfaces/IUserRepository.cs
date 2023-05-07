using needy_dto;
using needy_logic_abstraction.Parameters;

namespace needy_dataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserByCIAsync(string userCI);

        Task<bool> InsertUserAsync(InsertUserParameters parameters);

        Task<bool> InsertUserSkillAsync(int skilId);
    }
}
