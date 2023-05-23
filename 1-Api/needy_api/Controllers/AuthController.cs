using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;

namespace needy_api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        #region Properties and Fields

        private readonly IAuthLogic _authorizationLogic;

        #endregion

        #region Builders

        public AuthController(IAuthLogic authorizationLogic)
        {
            _authorizationLogic = authorizationLogic;
        }

        #endregion

        #region Implements

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string token = await _authorizationLogic.LoginAsync(parameters);

            if (token.IsNullOrEmpty())
            {
                return BadRequest("Nombre de usuario o contraseña incorrectos");
            }

            return Ok(token);
        }

        #endregion

    }
}
