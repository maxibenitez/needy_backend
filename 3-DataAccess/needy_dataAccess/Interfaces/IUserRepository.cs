using needy_dto;
using needy_logic_abstraction.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace needy_dataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserByCIAsync(int userCI);

        Task<bool> InsertUserAsync(InsertUserParameters parameters);
    }
}
