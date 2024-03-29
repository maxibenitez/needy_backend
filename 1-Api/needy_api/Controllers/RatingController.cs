﻿using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("get-ratings-by-user")]
        public async Task<IActionResult> GetUserRatingsAsync([FromBody] string userCI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return Ok(await _ratingLogic.GetUserRatingsAsync(userCI));
        }

        [HttpPost("insert-rating")]
        public async Task<IActionResult> InsertRatingAsync([FromBody] InsertRatingParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return await _ratingLogic.InsertRatingAsync(parameters) ? Ok() : BadRequest();
        }

        #endregion

    }

}
