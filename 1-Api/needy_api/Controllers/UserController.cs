using Microsoft.AspNetCore.Mvc;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;

namespace needy_api.Controllers
{
    [Route("api/users")]
    public class UserController : Controller
    {
        #region Properties and Fields

        private readonly IUserLogic _userLogic;

        #endregion

        #region Builders

        public UserController(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        #endregion

        #region Implements

        [HttpGet("get-users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            return Ok(await _userLogic.GetUsersAsync());
        }

        [HttpGet("get-user-by-ci")]
        public async Task<IActionResult> GetUserByCIAsync(int userCI)
        {
            return Ok(await _userLogic.GetUserByCIAsync(userCI));
        }

        [HttpPost("insert-user")]
        public async Task<IActionResult> InsertUserAsync([FromBody] InsertUserParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _userLogic.InsertUserAsync(parameters);
            
            return result ? Ok(result) : BadRequest();
        }

        #endregion
    }
}
