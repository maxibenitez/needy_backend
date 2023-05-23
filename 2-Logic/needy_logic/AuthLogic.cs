using needy_dataAccess.Interfaces;
using needy_dataAccess.Repositories;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace needy_logic
{
    public class AuthLogic : IAuthLogic
    {
        #region Properties and Fields

        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;
        private readonly IConfiguration _configuration;

        #endregion

        #region Builders

        public AuthLogic(IAuthorizationRepository authorizationRepository, 
            IUserRepository userRepository, 
            IUserContext userContext,
            IConfiguration configuration)
        {
            _authorizationRepository = authorizationRepository;
            _userRepository = userRepository;
            _userContext = userContext;
            _configuration = configuration;
        }

        #endregion

        #region Implements IAuthorizationLogic

        public async Task<string> LoginAsync(LoginParameters parameters)
        {
            User user = await _userRepository.GetUserByEmailAsync(parameters.Email);
            string token = "";

            if (user != null)
            {
                var result = BCrypt.Net.BCrypt.Verify(parameters.Password, user.Password);

                if (result)
                {
                    //_userContext.NewSession();
                    //SetSession(user);

                    return GenerateJwtToken(user);
                }

                return token;
            }

            return token;
        }

        #endregion

        #region Private Methods

        private void SetSession(User user)
        {
            Session userSession = new()
            {
                CI = user.CI,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                LoginDate = DateTime.Now
            };

            _userContext.SetUserSession(userSession);
        }

        private string GenerateJwtToken(User user)
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
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}
