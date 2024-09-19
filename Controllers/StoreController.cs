using GameStore.Dtos;
using GameStore.Models.Games;
using GameStore.Repositories.Store;
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
    }
}
