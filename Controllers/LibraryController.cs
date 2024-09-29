using GameStore.Models.Games;
using GameStore.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LibraryController : Controller
    {
        private ILibraryRepository repository;
        public LibraryController(ILibraryRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("GetLibraryByUserId")]
        public async Task<ActionResult<IEnumerable<Game>>> GetReviewsByGameId(int userId)
        {
            var result = await repository.GetLibraryByUserId(userId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("GetLibraryById")]
        public async Task<ActionResult<IEnumerable<Game>>> GetLibraryById(int id)
        {
            var result = await repository.GetLibraryById(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
