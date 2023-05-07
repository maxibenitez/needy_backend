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
        private readonly IUserRepository _userRepository;

        #endregion

        #region Builders

        public RatingLogic(IRatingRepository ratingRepository, IUserRepository userRepository)
        {
            _ratingRepository = ratingRepository;
            _userRepository = userRepository;
        }

        #endregion

        #region Implements IRatingLogic

        public Task<Rating> GetRatingByUserCiAsync(string userCi)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertRatingAsync(InsertRatingParameters parameters)
        {
            //Controlar el user
            return await _ratingRepository.InsertRatingAsync(parameters);
        }

        #endregion
    }
}
