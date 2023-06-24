using Microsoft.AspNetCore.Mvc;
using needy_logic_abstraction.Parameters;
using needy_logic_abstraction;
using Microsoft.AspNetCore.Authorization;
using needy_logic_abstraction.Enumerables;
using System.Net;

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

        [HttpPost("get-needs-by-skill-name")]
        public async Task<IActionResult> GetNeedsBySkillNameAsync([FromBody] string skillName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return Ok(await _needLogic.GetNeedsBySkillNameAsync(skillName));
        }

        [HttpGet("get-need-by-id/{needId}")]
        public async Task<IActionResult> GetNeedByIdAsync(int needId)
        {
            return Ok(await _needLogic.GetNeedByIdAsync(needId));
        }

        [HttpPost("get-user-created-needs")]
        public async Task<IActionResult> GetUserCreatedNeedsAsync([FromBody] string userCI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return Ok(await _needLogic.GetUserCreatedNeedsAsync(userCI));
        }

        [HttpPost("get-user-applied-needs")]
        public async Task<IActionResult> GetUserAppliedNeedsAsync([FromBody] string userCI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return Ok(await _needLogic.GetUserAppliedNeedsAsync(userCI));
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

        [HttpDelete("delete-need/{needId}")]
        public async Task<IActionResult> DeleteNeedAsync(int needId)
        {
            return await _needLogic.DeleteNeedAsync(needId) ? Ok() : BadRequest();
        }

        [HttpPost("apply-need/{needId}")]
        public async Task<IActionResult> ApplyNeedAsync(int needId)
        {
            ErrorStatus status = await _needLogic.ApplyNeedAsync(needId);

            switch (status)
            {
                case ErrorStatus.Success:
                    return Ok("Apply success");
                case ErrorStatus.IsNeedRequestor:
                    return BadRequest("Cannot apply to own need");
                case ErrorStatus.NotHasRequiredSkills:
                    return BadRequest("Not have the required skills");
                default:
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Upss...! An error has occurred");
            }
        }

        [HttpDelete("unapply-need/{needId}")]
        public async Task<IActionResult> UnapplyNeedAsync(int needId)
        {
            return await _needLogic.UnapplyNeedAsync(needId) ? Ok() : BadRequest();
        }

        [HttpPut("accept-applier")]
        public async Task<IActionResult> AcceptApplierAsync([FromBody] ManageApplierParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            ErrorStatus status = await _needLogic.AcceptApplierAsync(parameters);

            switch (status)
            {
                case ErrorStatus.Success:
                    return Ok("Applier accepted successfully");
                case ErrorStatus.ApplierNotExist:
                    return BadRequest("User not applied");
                case ErrorStatus.AcceptedApllierExist:
                    return BadRequest("Already exists an accepted applier");
                default:
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Upss...! An error has occurred");
            }
        }

        [HttpPut("decline-applier")]
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
