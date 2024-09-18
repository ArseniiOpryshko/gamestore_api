using AutoMapper;
using GameStore.Data;
using GameStore.Dtos;
using GameStore.Models.Games;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<Game> CreateGame(GameCreateDto gameDto) //, byte[] mainImage, List<byte[]> images
        {
            Game game = mapper.Map<Game>(gameDto);

            return game;
        }

    }
    public interface IStoreRepository
    {
        Task<IEnumerable<Game>> GetGames();
        Task<Game> CreateGame(GameCreateDto gameDto); //, byte[] mainImage, List<byte[]> images
        Task<Game> UpdateGame();
        Task<int> DeleteGame(string name);
    }
}
