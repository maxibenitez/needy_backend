using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;

namespace needy_logic
{
    public class RatingLogic : IRatingLogic
    {
        #region Properties and Fields

        private readonly IRatingRepository _ratingRepository;
        private readonly ITokenLogic _tokenLogic;

        #endregion

        #region Builders

        public RatingLogic(IRatingRepository ratingRepository, ITokenLogic tokenLogic)
        {
            _ratingRepository = ratingRepository;
            _tokenLogic = tokenLogic;
        }

        #endregion

        #region Implements IRatingLogic

        public async Task<IEnumerable<Rating>> GetUserRatingsAsync(string userCI)
        {
            return await _ratingRepository.GetUserRatingsAsync(userCI);
        }

        public async Task<bool> InsertRatingAsync(InsertRatingParameters parameters)
        {
            string userCI = await _tokenLogic.GetUserCIFromToken();
            
            return await _ratingRepository.InsertRatingAsync(userCI, parameters);
        }

        #endregion
    }
}
