using GameStore.Dtos.GameDtos;
using GameStore.Models.Games;
using GameStore.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoreController : Controller
    {
        private IStoreRepository repository;
        public StoreController(IStoreRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("GetGames")]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            var result = await repository.GetGames();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("CreateGame")]
        public async Task<ActionResult<int>> CreateGame(GameCreateDto gameDto)//, [FromForm] IFormFile mainImage, [FromForm] List<IFormFile> images
        {
            var result = await repository.CreateGame(gameDto);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpPut("UpdateGame")]
        public async Task<ActionResult<int>> UpdateGame(GameUpdateDto gameDto)
        {
            var result = await repository.UpdateGame(gameDto);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpDelete("DeleteGameByName")]
        public async Task<ActionResult<int>> DeleteGameByName(string name)
        {
            var result = await repository.DeleteGameByName(name);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpDelete("DeleteGameById")]
        public async Task<ActionResult<int>> DeleteGameById(int id)
        {
            var result = await repository.DeleteGameById(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
