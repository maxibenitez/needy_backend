using needy_dto;
using needy_logic_abstraction.Parameters;

namespace needy_dataAccess.Interfaces
{
    public interface IRatingRepository
    {
        Task<IEnumerable<Rating>> GetRatingByUserCiAsync(string userCi);

        Task<bool> InsertRatingAsync(InsertRatingParameters parameters);
    }
}
