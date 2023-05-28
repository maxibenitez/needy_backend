using Microsoft.AspNetCore.Mvc;
using needy_logic_abstraction.Parameters;
using needy_logic_abstraction;
using Microsoft.AspNetCore.Authorization;

namespace needy_api.Controllers
{
    [Route("api/needs")]
    [ApiController]
    [Authorize]
    public class NeedController : Controller
    {
        #region Properties and Fields

        private readonly INeedLogic _needLogic;

        #endregion

        #region Builders

        public NeedController(INeedLogic needLogic)
        {
            _needLogic = needLogic;
        }

        #endregion

        #region Implements

        [HttpGet("get-needs")]
        public async Task<IActionResult> GetNeedsAsync()
        {
            return Ok(await _needLogic.GetNeedsAsync());
        }

        [HttpGet("get-needs-by-skill/{skillId}")]
        public async Task<IActionResult> GetNeedsBySkillAsync(int skillId)
        {
            return Ok(await _needLogic.GetNeedsBySkillAsync(skillId));
        }

        [HttpGet("get-need-by-id/{needId}")]
        public async Task<IActionResult> GetNeedByIdAsync(int needId)
        {
            return Ok(await _needLogic.GetNeedByIdAsync(needId));
        }

        [HttpPost("insert-need")]
        public async Task<IActionResult> InsertNeedAsync([FromBody] InsertNeedParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return await _needLogic.InsertNeedAsync(parameters) ? Ok() : BadRequest();
        }

        [HttpPut("update-need/{needId}")]
        public async Task<IActionResult> UpdateNeedAsync(int needId, [FromBody] UpdateNeedParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return await _needLogic.UpdateNeedAsync(needId, parameters) ? Ok() : BadRequest();
        }

        [HttpDelete("delete-need/{needId}")]
        public async Task<IActionResult> DeleteNeedAsync(int needId)
        {
            return await _needLogic.DeleteNeedAsync(needId) ? Ok() : BadRequest();
        }

        [HttpPost("apply-need/{needId}")]
        public async Task<IActionResult> ApplyNeedAsync(int needId)
        {
            return await _needLogic.ApplyNeedAsync(needId) ? Ok() : BadRequest();
        }

        [HttpDelete("unapply-need/{needId}")]
        public async Task<IActionResult> UnapplyNeedAsync(int needId)
        {
            return await _needLogic.UnapplyNeedAsync(needId) ? Ok() : BadRequest();
        }

        [HttpPut("accept-applier/{needId}")]
        public async Task<IActionResult> AcceptApplierAsync(int needId, [FromBody] string applierCi)
        {
            return await _needLogic.AcceptApplierAsync(needId, applierCi) ? Ok() : BadRequest();
        }

        [HttpDelete("decline-applier/{needId}")]
        public async Task<IActionResult> DeclineApplierAsync(int needId, [FromBody] string applierCi)
        {
            return await _needLogic.DeclineApplierAsync(needId, applierCi) ? Ok() : BadRequest();
        }

        #endregion
    }
}
