using Microsoft.AspNetCore.Http;
using needy_dataAccess.Interfaces;
using needy_dataAccess.Repositories;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;
using System.IdentityModel.Tokens.Jwt;

namespace needy_logic
{
    public class RatingLogic : IRatingLogic
    {
        #region Properties and Fields

        private readonly IRatingRepository _ratingRepository;
        private readonly IUserLogic _userLogic;

        #endregion

        #region Builders

        public RatingLogic(IRatingRepository ratingRepository, IUserLogic userLogic)
        {
            _ratingRepository = ratingRepository;
            _userLogic = userLogic;
        }

        #endregion

        #region Implements IRatingLogic

        public async Task<IEnumerable<Rating>> GetUserRatingsAsync(string userCI)
        {
            List<RatingData> data = (await _ratingRepository.GetUserRatingsAsync(userCI)).ToList();
            List<Rating> ratings = new List<Rating>();

            foreach (RatingData rating in data)
            {
                ratings.Add(await RatingBuilderAsync(rating));
            }

            return ratings;
        }

        public async Task<bool> InsertRatingAsync(InsertRatingParameters parameters)
        {
            //Controlar el user
            return await _ratingRepository.InsertRatingAsync(parameters);
        }

        #endregion

        #region Private Methods

        private async Task<Rating> RatingBuilderAsync(RatingData data)
        {
            var rating = new Rating
            {
                Stars = data.Stars,
                Comment = data.Comment,
            };

            rating.Giver = await _userLogic.GetUserByCIAsync(data.GiverCI);

            return rating;
        }

        #endregion
    }
}
