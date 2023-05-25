using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;

namespace needy_api.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> GetUserByCIAsync([FromBody] string userCI)
        {
            return Ok(await _userLogic.GetUserByCIAsync(userCI));
        }

        [HttpPost("insert-user-skill")]
        public async Task<IActionResult> InsertUserSkillAsync([FromBody] int skillId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _userLogic.InsertUserSkillAsync(skillId);

            return result ? Ok() : BadRequest();
        }

        #endregion
    }
}
