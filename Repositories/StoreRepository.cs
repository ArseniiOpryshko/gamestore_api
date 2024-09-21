using AutoMapper;
using GameStore.Common;
using GameStore.Data;
using GameStore.Dtos;
using GameStore.Dtos.GameDtos;
using GameStore.Models.Games;
using GameStore.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GameStore.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly IMapper mapper;
        private readonly GameStoreContext context;

        public StoreRepository(GameStoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<OperationResult<int>> DeleteGameByName(string name)
        {
            Game? game = await context.Games.FirstOrDefaultAsync(x => x.Name == name);
            if (game == null)
            {
                return OperationResult<int>.FailureResult("Game with such name doesn't exist");
            }

            context.Games.Remove(game);
            await context.SaveChangesAsync();

            return OperationResult<int>.SuccessResult(game.Id);
        }

        public async Task<OperationResult<int>> DeleteGameById(int id)
        {
            Game? game = await context.Games.FindAsync(id);
            if (game == null)
            {
                return OperationResult<int>.FailureResult("Game with such id doesn't exist");
            }

            context.Games.Remove(game);
            await context.SaveChangesAsync();

            return OperationResult<int>.SuccessResult(id);
        }

        public async Task<OperationResult<IEnumerable<Game>>> GetGames()
        {
            var games = await context.Games
                .AsNoTracking()
                .Include(x => x.Tags)
                .Include(x => x.Photos)
                .Include(x => x.Reviews)
                .ToListAsync();

            if (!games.Any())
            {
                return OperationResult<IEnumerable<Game>>.FailureResult("There are no games in the store");
            }

            return OperationResult<IEnumerable<Game>>.SuccessResult(games);
        }

        public async Task<OperationResult<Game>> UpdateGame(GameUpdateDto gameDto)
        {
            Game? game = await context.Games
                .Include(g => g.Tags)
                .FirstOrDefaultAsync(x => x.Id == gameDto.Id);

            if (game == null)
            {
                return OperationResult<Game>.FailureResult("Game with such id doesn't exist");
            }

            if (gameDto.CompanyId != 0)
            {
                Company? company = await context.Companies.FirstOrDefaultAsync(x => x.Id == gameDto.CompanyId);
                if (company == null)
                {
                    return OperationResult<Game>.FailureResult("Company with such id doesn't exist");
                }
            }

            game = mapper.Map(gameDto, game);

            var tagsFromDb = await context.Tags
                .Where(x => gameDto.Tags.Contains(x.Name))
                .ToListAsync();

            var newTags = gameDto.Tags
                .Where(tagName => !tagsFromDb.Any(t => t.Name == tagName))
                .Select(tagName => new Tag { Name = tagName })
                .ToList();

            game.Tags = tagsFromDb.Concat(newTags).ToList();

            await context.SaveChangesAsync();

            return OperationResult<Game>.SuccessResult(game);
        }

        public async Task<OperationResult<int>> CreateGame(GameCreateDto gameDto) //, byte[] mainImage, List<byte[]> images
        {
            Game game = mapper.Map<Game>(gameDto);
            Company? company = await context.Companies.FirstOrDefaultAsync(x => x.Id == gameDto.CompanyId);
            if (company == null)
            {
                return OperationResult<int>.FailureResult("Company with such id doesn't exist");
            }
            game.ReleaseDate = DateTime.Now;

            var tagsFromDb = await context.Tags
                .Where(x => gameDto.Tags.Contains(x.Name))
                .ToListAsync();

            var newTags = gameDto.Tags
                .Where(tagName => !tagsFromDb.Any(t => t.Name == tagName))
                .Select(tagName => new Tag { Name = tagName })
                .ToList();

            game.Tags = tagsFromDb.Concat(newTags).ToList();

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            return OperationResult<int>.SuccessResult(game.Id);
        }

    }
    public interface IStoreRepository
    {
        Task<OperationResult<IEnumerable<Game>>> GetGames();
        Task<OperationResult<int>> CreateGame(GameCreateDto gameDto); //, byte[] mainImage, List<byte[]> images
        Task<OperationResult<Game>> UpdateGame(GameUpdateDto gameDto);
        Task<OperationResult<int>> DeleteGameByName(string name);
        Task<OperationResult<int>> DeleteGameById(int id);
    }
}
