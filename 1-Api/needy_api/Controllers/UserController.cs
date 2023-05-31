using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using needy_logic_abstraction;

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

        [HttpGet("get-users-by-skill/{skillId}")]
        public async Task<IActionResult> GetUsersBySkillAsync(int skillId)
        {
            return Ok(await _userLogic.GetUsersBySkillAsync(skillId));
        }

        [HttpGet("get-user-by-ci")]
        public async Task<IActionResult> GetUserByCIAsync([FromBody] string userCI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return Ok(await _userLogic.GetUserByCIAsync(userCI));
        }

        [HttpPost("insert-user-skill")]
        public async Task<IActionResult> InsertUserSkillAsync([FromBody] int skillId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return await _userLogic.InsertUserSkillAsync(skillId) ? Ok() : BadRequest();
        }

        #endregion
    }
}
