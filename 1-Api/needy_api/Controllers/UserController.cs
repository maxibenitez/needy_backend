using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using needy_logic;
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

        [HttpPost("insert-user-skills")]
        public async Task<IActionResult> InsertUserSkillsAsync([FromBody] List<int> skillsId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            await _userLogic.InsertUserSkillsAsync(skillsId);

            return Ok();
        }

        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return await _userLogic.UpdateUserAsync(parameters) ? Ok() : BadRequest();
        }

        [HttpPut("update-user-skills")]
        public async Task<IActionResult> UpdateUserSkillsAsync([FromBody] List<int> skillsId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            await _userLogic.UpdateUserSkillsAsync(skillsId);

            return Ok();
        }

        #endregion
    }
}
