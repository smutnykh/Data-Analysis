using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelnessAPI.Interfaces;
using TravelnessAPI.Models.Album;
using TravelnessAPI.Utils;

namespace TravelnessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        readonly IAlbumService albumService;

        public AlbumController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, string search = null, int page = 1, int pageSize = 1)
        {
            var result = await albumService.GetById(id, search, page, pageSize);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }
        [HttpGet("user/{userId}/{sightseeingId}")]
        public async Task<IActionResult> GetUserAlbums(int userId, int sightseeingId)
        {
            var result = await albumService.GetUserAlbums(userId, sightseeingId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpPost("add/{id}/{sightseeingId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = Role.StandartUser)]
        public async Task<IActionResult> AddSightseeing(int id, int sightseeingId)
        {
            var result = await albumService.AddSightseeing(id, sightseeingId, int.Parse(HttpContext.User.FindFirst("ID").Value));
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpPost("remove/{id}/{sightseeingId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = Role.StandartUser)]
        public async Task<IActionResult> RemoveSightseeing(int id, int sightseeingId)
        {
            var result = await albumService.RemoveSightseeing(id, sightseeingId, int.Parse(HttpContext.User.FindFirst("ID").Value));
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = Role.StandartUser)]
        public async Task<IActionResult> Create([FromForm] AlbumCreateViewModel model)
        {
            var result = await albumService.Create(model, int.Parse(HttpContext.User.FindFirst("ID").Value));
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = Role.StandartUser)]
        public async Task<IActionResult> Edit([FromForm] AlbumEditViewModel model)
        {
            var result = await albumService.Edit(model, int.Parse(HttpContext.User.FindFirst("ID").Value));
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = Role.StandartUser)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await albumService.Delete(id, int.Parse(HttpContext.User.FindFirst("ID").Value));
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }
    }
}
