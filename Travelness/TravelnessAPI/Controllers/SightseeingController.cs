using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TravelnessAPI.Interfaces;
using TravelnessAPI.Models.Sightseeing;
using TravelnessAPI.Utils;

namespace TravelnessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SightseeingController : ControllerBase
    {
        private readonly ISightseeingService sightseeingService;

        public SightseeingController(ISightseeingService sightseeingService)
        {
            this.sightseeingService = sightseeingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await sightseeingService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpGet("count")]
        public async Task<IActionResult> Count()
        {
            var result = await sightseeingService.Count();
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpGet("params")]
        public async Task<IActionResult> GetByPage([FromQuery] string[] countries, [FromQuery] string[] areas, string search = null, int page = 1, int pageSize = 1)
        {
            var result = await sightseeingService.GetByPage(countries, areas, search, page, pageSize);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await sightseeingService.GetById(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetByIdWithTours(int id, [FromQuery] string[] companies, double minPrice, double maxPrice, string order, int page, int pageSize)
        {
            var result = await sightseeingService.GetByIdWithTours(id, companies, minPrice, maxPrice, order, page, pageSize);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Create([FromForm] SightseeingCreateEditViewModel model)
        {
            var result = await sightseeingService.Create(model);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Edit([FromForm] SightseeingCreateEditViewModel model)
        {
            var result = await sightseeingService.Edit(model);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await sightseeingService.Delete(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }
    }
}
