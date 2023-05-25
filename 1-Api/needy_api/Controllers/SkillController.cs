using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using needy_logic_abstraction;

namespace needy_api.Controllers
{
    [Route("api/skills")]
    [ApiController]
    [Authorize]
    public class SkillController : Controller
    {
        #region Properties and Fields

        private readonly ISkillLogic _skillLogic;

        #endregion

        #region Builders

        public SkillController(ISkillLogic skillLogic)
        {
            _skillLogic = skillLogic;
        }

        #endregion

        #region Implements

        [HttpGet("get-skills")]
        public async Task<IActionResult> GetUSkillsAsync()
        {
            return Ok(await _skillLogic.GetSkillsAsync());
        }


        #endregion
    }
}
