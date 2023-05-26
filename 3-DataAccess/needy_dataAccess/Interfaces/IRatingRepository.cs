using needy_dto;
using needy_logic_abstraction.Parameters;

namespace needy_dataAccess.Interfaces
{
    public interface IRatingRepository
    {
        Task<IEnumerable<Rating>> GetUserRatingsAsync(string userCI);

        Task<bool> InsertRatingAsync(string giverCI, InsertRatingParameters parameters);
    }
}
