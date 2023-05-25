using Microsoft.AspNetCore.Http;
using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction;
using Npgsql;
using System.IdentityModel.Tokens.Jwt;

namespace needy_logic
{
    public class UserLogic : IUserLogic
    {
        #region Properties and Fields

        private readonly IUserRepository _userRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IRatingLogic _ratingLogic;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Builders

        public UserLogic(IUserRepository userRepository,
            ISkillRepository skillRepository,
            IRatingLogic ratingLogic,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _skillRepository = skillRepository;
            _ratingLogic = ratingLogic;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Implements IUserLogic

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            List<UserData> data = (await _userRepository.GetUsersAsync()).ToList();
            List<User> users = new List<User>();

            foreach (UserData user in data)
            {
                users.Add(await UserBuilderAsync(user));
            }

            return users;
        }

        public async Task<IEnumerable<User>> GetUsersBySkillAsync(int skillId)
        {
            List<UserData> data = (await _userRepository.GetUsersBySkillAsync(skillId)).ToList();
            List<User> users = new List<User>();

            foreach (UserData user in data)
            {
                users.Add(await UserBuilderAsync(user));
            }

            return users;
        }

        public async Task<User> GetUserByCIAsync(string userCI)
        {
            UserData data = await _userRepository.GetUserByCIAsync(userCI);

            return await UserBuilderAsync(data);
        }

        public async Task<bool> InsertUserSkillAsync(int skillId)
        {
            string userCI = await GetUserCIFromToken();

            return await _userRepository.InsertUserSkillAsync(userCI, skillId);
        }

        #endregion

        #region Private Methods

        private async Task<string> GetUserCIFromToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                var authorizationHeader = httpContext.Request.Headers["Authorization"];
                var token = authorizationHeader.ToString().Replace("Bearer ", string.Empty);

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                if (jwtToken.Payload.TryGetValue("CI", out var userCI))
                {
                    return userCI.ToString();
                }
            }

            return null;
        }

        private async Task<User> UserBuilderAsync(UserData data)
        {
            var user = new User
            {
                CI = data.CI,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Address = data.Address,
                Zone = data.Zone,
                Phone = data.Phone,
                BirthDate = data.BirthDate,
            };

            if(data.SkillId is not null)
            {
                user.Skill = await _skillRepository.GetSkillByIdAsync((int)data.SkillId);
            }

            
            user.Ratings = (await _ratingLogic.GetUserRatingsAsync(data.CI)).ToList();
            user.AvgRating = await GetRatingAverageAsync(user.Ratings);

            return user;
        }

        private async Task<double> GetRatingAverageAsync(IEnumerable<Rating> ratings)
        {
            double total = 0;

            foreach(Rating rating in ratings)
            {
                total += rating.Stars;
            }

            return total / ratings.Count();
        }

        #endregion
    }
}
