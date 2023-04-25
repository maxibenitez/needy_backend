using Microsoft.AspNetCore.Mvc;
using needy_logic;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;

namespace needy_api.Controllers
{
    [Route("api/raitings")]
    public class RaitingController : Controller
    {
        #region Properties and Fields

        private readonly IRaitingLogic _raitingLogic;
            
        #endregion

        #region Builders

        public RaitingController(IRaitingLogic raitingLogic)
        {
            _raitingLogic = raitingLogic;
        }

        #endregion

        #region Implements

        [HttpPost("insert-raiting")]
        public async Task<IActionResult> InsertRaitingAsync([FromBody] InsertRaitingParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _raitingLogic.InsertRaitingAsync(parameters);

            return result ? Ok(result) : BadRequest();
        }

        #endregion

    }

}
