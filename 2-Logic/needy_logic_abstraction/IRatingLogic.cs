using needy_dto;
using needy_logic_abstraction.Parameters;

namespace needy_logic_abstraction
{
    public interface IRatingLogic
    {
        //Task<decimal> GetRatingByUserCiAsync(string userCi);

        Task<bool> InsertRatingAsync(InsertRatingParameters parameters);
    }
}
