using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelnessAPI.Interfaces;
using TravelnessAPI.Utils;

namespace TravelnessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IRateService rateService;

        public RateController(IRateService rateService)
        {
            this.rateService = rateService;
        }

        [HttpGet("average/{sightseeingId}")]
        public async Task<IActionResult> GetSightseeingRate(int sightseeingId)
        {
            var result = await rateService.GetSightseeingRate(sightseeingId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpGet("{sightseeingId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = Role.StandartUser)]
        public async Task<IActionResult> GetUserRating(int sightseeingId)
        {
            var result = await rateService.GetUserRating(sightseeingId, int.Parse(HttpContext.User.FindFirst("ID").Value));
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpPost("{sightseeingId}/{rating}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = Role.StandartUser)]
        public async Task<IActionResult> Add(int sightseeingId, byte rating)
        {
            var result = await rateService.Add(sightseeingId, int.Parse(HttpContext.User.FindFirst("ID").Value), rating);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpDelete("{sightseeingId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = Role.StandartUser)]
        public async Task<IActionResult> Remove(int sightseeingId)
        {
            var result = await rateService.Remove(sightseeingId, int.Parse(HttpContext.User.FindFirst("ID").Value));
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }
    }
}
