using AutoMapper;
using GameStore.Common;
using GameStore.Data;
using GameStore.Dtos;
using GameStore.Models.Games;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Game> UpdateGame()
        {
            throw new NotImplementedException();
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

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            return OperationResult<int>.SuccessResult(game.Id);    
        }

    }
    public interface IStoreRepository
    {
        Task<IEnumerable<Game>> GetGames();
        Task<OperationResult<int>> CreateGame(GameCreateDto gameDto); //, byte[] mainImage, List<byte[]> images
        Task<Game> UpdateGame();
        Task<int> DeleteGame(string name);
    }
}
