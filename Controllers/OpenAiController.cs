using GameStore.Models.Games;
using GameStore.Models.Users;
using GameStore.Repositories;
using GenerativeAI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace GameStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OpenAiController : Controller
    {
        private readonly IOpenAiRepository repository;

        public OpenAiController(IOpenAiRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("GetResponseOnText")]
        public async Task<ActionResult<string>> GetResponseOnText(string text)
        {
            var result = await repository.GetResponseOnText(text);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);         
        }
        [HttpGet("GetResponseRecommendation")]
        public async Task<ActionResult<string>> GetResponseRecommendation(int userId)
        {
            var result = await repository.GetResponseRecommendation(userId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
