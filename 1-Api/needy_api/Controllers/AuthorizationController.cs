using Microsoft.AspNetCore.Mvc;
using needy_dto;
using needy_logic;
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
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return await _authorizationLogic.AuthenticateAsync(parameters) ? Ok() : BadRequest("Nombre de usuario o contraseña incorrectos");
        }

        #endregion

    }
}
