using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TravelnessAPI.Interfaces;
using TravelnessAPI.Models.Tour;
using TravelnessAPI.Utils;

namespace TravelnessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourController : ControllerBase
    {
        private readonly ITourService tourService;

        public TourController(ITourService tourService)
        {
            this.tourService = tourService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await tourService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await tourService.GetById(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = Role.BusinessUser)]
        public async Task<IActionResult> Create([FromForm]TourCreateViewModel model)
        {
            var result = await tourService.Create(int.Parse(HttpContext.User.FindFirst("ID").Value), model);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = Role.BusinessUser)]
        public async Task<IActionResult> Edit([FromForm] TourEditViewModel model)
        {
            var result = await tourService.Edit(int.Parse(HttpContext.User.FindFirst("ID").Value), model);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = Role.BusinessUser)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await tourService.Delete(int.Parse(HttpContext.User.FindFirst("ID").Value), id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }
    }
}
