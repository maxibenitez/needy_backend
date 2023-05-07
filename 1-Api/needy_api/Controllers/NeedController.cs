using Microsoft.AspNetCore.Mvc;
using needy_logic_abstraction.Parameters;
using needy_logic_abstraction;

namespace needy_api.Controllers
{
    [Route("api/needs")]
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

        [HttpPost("get-needs")]
        public async Task<IActionResult> GetNeedsAsync()
        {
            return Ok();
        }

        #endregion
    }
}
