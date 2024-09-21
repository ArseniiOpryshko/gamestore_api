using GameStore.Dtos.ReviewDtos;
using GameStore.Models.Games;
using GameStore.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : Controller
    {
        private IReviewRepository repository;
        public ReviewController(IReviewRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet("GetReviewsByGameId")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByGameId(int gameId)
        {
            var result = await repository.GetReviewsByGameId(gameId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpPost]
        public async Task<ActionResult<int>> AddReview(AddReviewDto reviewDto)
        {
            var result = await repository.AddReview(reviewDto);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpPut]
        public async Task<ActionResult<int>> UpdateReview(UpdateReviewDto reviewDto)
        {
            var result = await repository.UpdateReview(reviewDto);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpDelete]
        public async Task<ActionResult<int>> DeleteReviewById(int id)
        {
            var result = await repository.DeleteReview(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
