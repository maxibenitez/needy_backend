using Microsoft.AspNetCore.Mvc;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;

namespace needy_api.Controllers
{
    [Route("api/authorization")]
    public class AuthorizationController : Controller
    {
        #region Properties and Fields

        private readonly IAuthorizationLogic _authorizationLogic;

        #endregion

        #region Builders

        public AuthorizationController(IAuthorizationLogic authorizationLogic)
        {
            _authorizationLogic = authorizationLogic;
        }

        #endregion

        #region Implements

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationParameters parameters)
        {
            return Ok();
        }

        #endregion

    }
}
