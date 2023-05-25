using needy_dto;
using needy_logic_abstraction.Parameters;

namespace needy_logic_abstraction
{
    public interface IRatingLogic
    {
        Task<IEnumerable<Rating>> GetUserRatingsAsync(string userCI);

        Task<bool> InsertRatingAsync(InsertRatingParameters parameters);
    }
}
