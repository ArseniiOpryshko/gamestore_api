using GameStore.Data;
using GameStore.Models.Users;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GameStore.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly GameStoreContext context;

        public UserRepository(GameStoreContext context)
        {
            this.context = context;
        }
        public async Task<User> Create(User user)
        {
            await context.Users.AddAsync(user);
            user.Id = await context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetByEmail(string email)
        {
            User? user = await context.Users
                .Include(x => x.Cart)
                .Where(x => x.Email.Equals(email))
                .FirstOrDefaultAsync();
            return user;
        }
    }

    public interface IUserRepository
    {
        Task<User> GetByEmail(string email);
        Task<User> Create(User user);
    }
}
