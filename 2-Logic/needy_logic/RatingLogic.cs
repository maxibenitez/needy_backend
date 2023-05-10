using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;
using System.Collections.Generic;

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

        public async Task<decimal> GetRatingByUserCiAsync(string userCi)
        {
            
            IEnumerable<Rating> ratingsEnum = await _ratingRepository.GetRatingByUserCiAsync(userCi);
            List<Rating> ratings = ratingsEnum.ToList();
            decimal ratingAux = 0;
            foreach (Rating rating in ratingsEnum)
            {
                ratingAux += rating.RatingValue;
            }
            decimal avg = ratingAux / ratings.Count();
            return avg;
            
            throw new NotImplementedException();
        }

        public async Task<bool> InsertRatingAsync(InsertRatingParameters parameters)
        {
            //Controlar el user
            User requestor = await _userRepository.GetUserByCIAsync(parameters.CiRequestor);
            User helper = await _userRepository.GetUserByCIAsync(parameters.CiHelper);
            if(requestor == null || helper == null) {
                //Capaz podria tirar excepción acá
                return false;
            }
            return await _ratingRepository.InsertRatingAsync(parameters);
        }

        #endregion
    }
}
