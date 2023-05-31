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

        [HttpPut("update-need")]
        public async Task<IActionResult> UpdateNeedAsync([FromBody] UpdateNeedParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return await _needLogic.UpdateNeedAsync(parameters) ? Ok() : BadRequest();
        }

        [HttpDelete("delete-need")]
        public async Task<IActionResult> DeleteNeedAsync([FromBody] int needId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return await _needLogic.DeleteNeedAsync(needId) ? Ok() : BadRequest();
        }

        [HttpPost("apply-need")]
        public async Task<IActionResult> ApplyNeedAsync([FromBody] int needId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return await _needLogic.ApplyNeedAsync(needId) ? Ok() : BadRequest();
        }

        [HttpDelete("unapply-need")]
        public async Task<IActionResult> UnapplyNeedAsync([FromBody] int needId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return await _needLogic.UnapplyNeedAsync(needId) ? Ok() : BadRequest();
        }

        [HttpPut("accept-applier")]
        public async Task<IActionResult> AcceptApplierAsync([FromBody] ManageApplierParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return await _needLogic.AcceptApplierAsync(parameters) ? Ok() : BadRequest();
        }

        [HttpDelete("decline-applier")]
        public async Task<IActionResult> DeclineApplierAsync([FromBody] ManageApplierParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return await _needLogic.DeclineApplierAsync(parameters) ? Ok() : BadRequest();
        }

        #endregion
    }
}
