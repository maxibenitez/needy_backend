using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;

namespace needy_logic
{
    public class UserLogic : IUserLogic
    {
        #region Properties and Fields

        private readonly IUserRepository _userRepository;

        private readonly IRatingRepository _ratingRepository;

        #endregion

        #region Builders

        public UserLogic(IUserRepository userRepository, IRatingRepository ratingRepository)
        {
            _userRepository = userRepository;
            _ratingRepository = ratingRepository;
        }

        #endregion

        #region Implements IUserLogic
                
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task<User> GetUserByCIAsync(string userCI)
        {   
            User user = await _userRepository.GetUserByCIAsync(userCI);

            user.Rating = await this.getUserRating(userCI); ;

            return user;

        }

        public async Task<bool> InsertUserAsync(InsertUserParameters parameters)
        {
            return await _userRepository.InsertUserAsync(parameters);
        }

        public Task<bool> InsertUserSkillAsync(int skilId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private async Task<UserRating> getUserRating(string userCI)
        {
            IEnumerable<Rating> ratingsEnum = await _ratingRepository.GetRatingByUserCiAsync(userCI);
            List<Rating> ratings = ratingsEnum.ToList();

            decimal ratingAux = 0;
            List<string> comments = new List<string>();

            foreach (Rating rating in ratingsEnum)
            {
                ratingAux += rating.RatingValue;
                comments.Add(rating.Comment);
               
            }

            decimal avg = ratingAux / ratings.Count();

            UserRating userRating = new UserRating();
            userRating.Average = avg;
            userRating.Comments = comments;

            return userRating;
        }
        #endregion
    }
}
