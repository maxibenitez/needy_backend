using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;

namespace needy_api.Controllers
{
    [Route("api/ratings")]
    [ApiController]
    [Authorize]
    public class RatingController : Controller
    {
        #region Properties and Fields

        private readonly IRatingLogic _ratingLogic;
            
        #endregion

        #region Builders

        public RatingController(IRatingLogic ratingLogic)
        {
            _ratingLogic = ratingLogic;
        }

        #endregion

        #region Implements

        [HttpPost("insert-rating")]
        public async Task<IActionResult> InsertRatingAsync([FromBody] InsertRatingParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _ratingLogic.InsertRatingAsync(parameters);

            return result ? Ok(result) : BadRequest();
        }

        #endregion

    }

}
