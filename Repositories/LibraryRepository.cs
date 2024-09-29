using GameStore.Common;
using GameStore.Data;
using GameStore.Models.Games;
using GameStore.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Repositories
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly GameStoreContext context;
        public LibraryRepository(GameStoreContext context)
        {
            this.context = context;
        }

        public async Task<OperationResult<List<Game>>> GetLibraryById(int id)
        {
            Library? library = await context.Libraries
                .Include(x => x.Games)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (library == null)
            {
                return OperationResult<List<Game>>.FailureResult("Library with such id doesn't exist");
            }

            return OperationResult<List<Game>>.SuccessResult(library.Games.ToList());
        }

        public async Task<OperationResult<List<Game>>> GetLibraryByUserId(int userId)
        {
            Library? library = await context.Libraries
                .Include(x => x.Games)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (library == null)
            {
                return OperationResult<List<Game>>.FailureResult("Library with such UserId doesn't exist");
            }

            return OperationResult<List<Game>>.SuccessResult(library.Games.ToList());
        }
    }
    public interface ILibraryRepository
    {
        public Task<OperationResult<List<Game>>> GetLibraryByUserId(int userId);
        public Task<OperationResult<List<Game>>> GetLibraryById(int id);
    }
}
