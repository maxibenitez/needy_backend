using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using needy_logic_abstraction;
using needy_logic_abstraction.Enumerables;
using needy_logic_abstraction.Parameters;
using System.Net;

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
                return BadRequest(ModelState.Values);
            }

            string token = await _authorizationLogic.LoginAsync(parameters);

            if (token.IsNullOrEmpty())
            {
                return BadRequest("Nombre de usuario o contraseña incorrectos");
            }

            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            ErrorStatus status = await _authorizationLogic.RegisterAsync(parameters);

            switch (status)
            {
                case ErrorStatus.Success:
                    return Ok("Registro exitoso");
                case ErrorStatus.UserAlreadyExist:
                    return BadRequest("Ya existe un usuario registrado con esa CI");
                case ErrorStatus.EmailAlreadyExist:
                    return BadRequest("Ya existe un usuario registrado con ese email");
                default:
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Ha ocurrido un error");
            }
        }

        #endregion

    }
}
