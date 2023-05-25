using needy_dto;
using needy_logic_abstraction.Parameters;

namespace needy_dataAccess.Interfaces
{
    public interface IRatingRepository
    {
        Task<IEnumerable<RatingData>> GetUserRatingsAsync(string userCI);

        Task<bool> InsertRatingAsync(InsertRatingParameters parameters);
    }
}
