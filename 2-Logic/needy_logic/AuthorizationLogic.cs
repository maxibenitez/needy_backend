using needy_dataAccess.Interfaces;
using needy_dataAccess.Repositories;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;

namespace needy_logic
{
    public class AuthorizationLogic : IAuthorizationLogic
    {
        #region Properties and Fields

        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IUserRepository _userRepository;
        private IUserContext _userContext;

        #endregion

        #region Builders

        public AuthorizationLogic(IAuthorizationRepository authorizationRepository, IUserRepository userRepository, IUserContext userContext)
        {
            _authorizationRepository = authorizationRepository;
            _userRepository = userRepository;
            _userContext = userContext;
        }

        #endregion

        #region Implements IAuthorizationLogic

        public async Task<bool> AuthenticateAsync(AuthenticationParameters parameters)
        {
            User user = await _userRepository.GetUserByEmailAsync(parameters.Email);

            if (user != null)
            {
                var result = BCrypt.Net.BCrypt.Verify(parameters.Password, user.Password);

                if (result)
                {
                    _userContext.NewSession();
                    SetSession(user);

                    return true;
                }

                return false;
            }

            return false;
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

        #endregion
    }
}
