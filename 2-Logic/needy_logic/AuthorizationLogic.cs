using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;

namespace needy_logic
{
    public class AuthorizationLogic : IAuthorizationLogic
    {
        #region Properties and Fields

        private readonly IAuthorizationRepository _authorizationRepository;

        #endregion

        #region Builders

        public AuthorizationLogic(IAuthorizationRepository authorizationRepository)
        {
            _authorizationRepository = authorizationRepository;
        }

        #endregion

        #region Implements IAuthorizationLogic

        public async Task<User> AuthenticateAsync(AuthenticationParameters parameters)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
