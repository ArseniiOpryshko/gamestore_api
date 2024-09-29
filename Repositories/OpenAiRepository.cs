using GameStore.Common;
using GameStore.Data;
using GameStore.Models.Games;
using GameStore.Models.Users;
using GenerativeAI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace GameStore.Repositories
{
    public class OpenAiRepository : IOpenAiRepository
    {
        private readonly IConfiguration configuration;
        private readonly GameStoreContext context;

        public OpenAiRepository(IConfiguration configuration, GameStoreContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }

        public async Task<OperationResult<string>> GetResponseOnText(string text)
        {
            var model = new GenerativeModel(configuration.GetValue<string>("GeminiApi:Key"));

            var response = await model.GenerateContentAsync(text);
            if (response == null)
            {
                return OperationResult<string>.FailureResult("Fail in getting answer");
            }

            return OperationResult<string>.SuccessResult(response);
        }

        public async Task<OperationResult<string>> GetResponseRecommendation(int userId)
        {
            var model = new GenerativeModel(configuration.GetValue<string>("GeminiApi:Key"));

            User? user = await context.Users
                .Include(x => x.Library)
                .ThenInclude(x => x!.Games)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return OperationResult<string>.FailureResult("User with such Id doesn't exist");
            }

            string promt;

            if (user.Library!.Games.Any())
            {
                promt = "What computer games can you recommend based on my library. It has: ";
                promt += string.Join(", ", user.Library!.Games.Select(x => x.Name));
                promt += ". Give a list of 6 games with descriprions. And write what game from the library it looks like";
            }
            else
            {
                promt = "What computer games can you recommend based on current trends. Give a list of 6 games with descriprions.";
            }

            var response = await model.GenerateContentAsync(promt);

            if (response == null)
            {
                return OperationResult<string>.FailureResult("Fail in getting answer");
            }

            return OperationResult<string>.SuccessResult(response);
        }
    }
    public interface IOpenAiRepository
    {
        public Task<OperationResult<string>> GetResponseOnText(string text);
        public Task<OperationResult<string>> GetResponseRecommendation(int userId);
    }
}
