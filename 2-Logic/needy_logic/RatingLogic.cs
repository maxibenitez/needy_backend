using Microsoft.AspNetCore.Http;
using needy_dataAccess.Interfaces;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Builders

        public RatingLogic(IRatingRepository ratingRepository, IUserLogic userLogic, IHttpContextAccessor httpContextAccessor)
        {
            _ratingRepository = ratingRepository;
            _userLogic = userLogic;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Implements IRatingLogic

        public async Task<IEnumerable<Rating>> GetUserRatingsAsync(string userCI)
        {
            return await _ratingRepository.GetUserRatingsAsync(userCI);
        }

        public async Task<bool> InsertRatingAsync(InsertRatingParameters parameters)
        {
            string userCI = await GetUserCIFromToken();
            
            return await _ratingRepository.InsertRatingAsync(userCI, parameters);
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

        #endregion
    }
}
