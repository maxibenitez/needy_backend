using Microsoft.AspNetCore.Mvc;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Enumerables;
using needy_logic_abstraction.Parameters;
using System.Net;
using System.Text.Json;

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

            Session userSession = await _authorizationLogic.LoginAsync(parameters);

            if (userSession == null)
            {
                return BadRequest("Invalid email or password");
            }

            return Ok(userSession);
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
                    return Ok(JsonSerializer.Serialize("Success signup"));
                case ErrorStatus.UserAlreadyExist:
                    return BadRequest("Already exists a registered user with that CI");
                case ErrorStatus.EmailAlreadyExist:
                    return BadRequest("Already exists a registered user with that email");
                default:
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Upss...! An error has occurred");
            }
        }

        #endregion

    }
}
