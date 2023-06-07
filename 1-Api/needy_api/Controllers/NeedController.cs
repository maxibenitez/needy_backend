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

            return await _needLogic.InsertNeedAsync(parameters) ? Ok("Solicitud de necesidad creada con éxito") : BadRequest("Ha ocurrido un error");
        }

        [HttpPut("update-need")]
        public async Task<IActionResult> UpdateNeedAsync([FromBody] UpdateNeedParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return await _needLogic.UpdateNeedAsync(parameters) ? Ok("Solicitud de necesidad actualizada con éxito") : BadRequest("Ha ocurrido un error");
        }

        [HttpDelete("delete-need")]
        public async Task<IActionResult> DeleteNeedAsync([FromBody] int needId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return await _needLogic.DeleteNeedAsync(needId) ? Ok("Solicitud de necesidad borrada con éxito") : BadRequest("Ha ocurrido un error");
        }

        [HttpPost("apply-need")]
        public async Task<IActionResult> ApplyNeedAsync([FromBody] int needId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            ErrorStatus status = await _needLogic.ApplyNeedAsync(needId);

            switch (status)
            {
                case ErrorStatus.Success:
                    return Ok("Se ha aplicado con éxito");
                case ErrorStatus.IsNeedRequestor:
                    return BadRequest("No puede aplicarse a una solicitud propia");
                case ErrorStatus.NotHasRequiredSkills:
                    return BadRequest("No cumple con las habilidades requeridas");
                default:
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Ha ocurrido un error");
            }
        }

        [HttpDelete("unapply-need")]
        public async Task<IActionResult> UnapplyNeedAsync([FromBody] int needId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return await _needLogic.UnapplyNeedAsync(needId) ? Ok("Se ha desaplicado con éxito") : BadRequest("Ha ocurrido un error");
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
                    return Ok("Usuario aceptado con éxito");
                case ErrorStatus.ApplierNotExist:
                    return BadRequest("Este usuario no está aplicado");
                case ErrorStatus.AcceptedApllierExist:
                    return BadRequest("Ya existe un usuario aceptado");
                default:
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Ha ocurrido un error");
            }
        }

        [HttpDelete("decline-applier")]
        public async Task<IActionResult> DeclineApplierAsync([FromBody] ManageApplierParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return await _needLogic.DeclineApplierAsync(parameters) ? Ok("Usuario desaplicado con éxito") : BadRequest("Ha ocurrido un error");
        }

        #endregion
    }
}
