using AutoMapper;
using GameStore.Common;
using GameStore.Data;
using GameStore.Dtos;
using GameStore.Models.Games;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GameStore.Repositories.Store
{
    public class StoreRepository : IStoreRepository
    {
        private readonly IMapper mapper;
        private readonly GameStoreContext context;

        public StoreRepository(GameStoreContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<int> DeleteGame(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Game>> GetGames()
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<Game>> UpdateGame(GameUpdateDto gameDto)
        {
            Game game = await context.Games
                .Include(g => g.Tags)
                .FirstOrDefaultAsync(x=>x.Id == gameDto.Id);

            if (game == null)
            {
                return OperationResult<Game>.FailureResult("Game with such id doesn't exist");
            }

            if (gameDto.CompanyId != 0)
            {
                Company company = await context.Companies.FirstOrDefaultAsync(x => x.Id == gameDto.CompanyId);
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
            Company company = await context.Companies.FirstOrDefaultAsync(x => x.Id == gameDto.CompanyId);
            if(company == null)
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
        Task<IEnumerable<Game>> GetGames();
        Task<OperationResult<int>> CreateGame(GameCreateDto gameDto); //, byte[] mainImage, List<byte[]> images
        Task<OperationResult<Game>> UpdateGame(GameUpdateDto gameDto);
        Task<int> DeleteGame(string name);
    }
}
