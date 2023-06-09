using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using needy_dto;
using needy_logic_abstraction;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace needy_logic
{
    public class TokenLogic : ITokenLogic
    {
        #region Properties and Fields

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        #endregion

        #region Builders

        public TokenLogic(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        #endregion

        #region Implements ITokenLogic

        public async Task<string> GenerateJwtToken(UserData user)
        {
            var claims = new List<Claim>
            {
                new Claim("CI", user.CI),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Token"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                _configuration["JWT:Issuer"],
                _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(7200),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GetUserCIFromToken()
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
