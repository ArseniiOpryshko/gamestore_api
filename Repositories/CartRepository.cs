using GameStore.Common;
using GameStore.Data;
using GameStore.Models.Games;
using GameStore.Models.Orders;
using GameStore.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly GameStoreContext context;

        public CartRepository(GameStoreContext context)
        {
            this.context = context;
        }

        public async Task<OperationResult<int>> AddToCart(int gameId, int cartId)
        {
            Game? game = await context.Games.FindAsync(gameId);

            if (game == null)
            {
                return OperationResult<int>.FailureResult("Game with such id doesn't exist");
            }

            Cart? cart = await context.Carts
                .Include(x => x.Orders)
                .FirstOrDefaultAsync(x => x.Id == cartId);

            if (cart == null)
            {
                return OperationResult<int>.FailureResult("Cart with such id doesn't exist");
            }

            if (!cart.Orders.Any())
            {
                cart.Orders = new List<Order>();
            }

            Order order = new Order()
            {
                OrderDate = DateTime.Now,
                Game = game
            };

            cart.Orders.Add(order);

            await context.SaveChangesAsync();

            return OperationResult<int>.SuccessResult(order.Id);
        }

        public async Task<OperationResult<IEnumerable<Order>>> GetOrdersFromCartById(int id)
        {
            Cart? cart = await context.Carts
                .AsNoTracking()
                .Include(x => x.Orders)
                .ThenInclude(x => x.Game)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (cart == null)
            {
                return OperationResult<IEnumerable<Order>>.FailureResult("Cart with such id doesn't exist");
            }

            return OperationResult<IEnumerable<Order>>.SuccessResult(
                cart.Orders
                    .OrderBy(x => x.OrderDate)
                    .ToList()
                );
        }

        public async Task<OperationResult<IEnumerable<Order>>> GetOrdersFromCartByUserId(int userId)
        {
            Cart? cart = await context.Carts
                .AsNoTracking()
                .Include(x => x.Orders)
                .ThenInclude(x => x.Game)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (cart == null)
            {
                return OperationResult<IEnumerable<Order>>.FailureResult("Cart with such id doesn't exist");
            }

            return OperationResult<IEnumerable<Order>>.SuccessResult(
                cart.Orders
                    .OrderBy(x => x.OrderDate)
                    .ToList()
                );
        }

        public async Task<OperationResult<int>> RemoveFromCart(int gameId, int cartId)
        {
            Cart? cart = await context.Carts
                .Include(x => x.Orders)
                .FirstOrDefaultAsync(x => x.Id == cartId);

            if (cart == null)
            {
                return OperationResult<int>.FailureResult("Cart with such id doesn't exist");
            }

            if (!cart.Orders.Any())
            {
                return OperationResult<int>.FailureResult("Cart is clear");
            }

            Order? order = cart.Orders.Where(x => x.GameId == gameId).FirstOrDefault();
            if (order == null)
            {
                return OperationResult<int>.FailureResult("Order with this game doesn't exist");
            }

            cart.Orders.Remove(order);
            context.Order.Remove(order);
            await context.SaveChangesAsync();

            return OperationResult<int>.SuccessResult(order.Id);
        }

        public async Task<OperationResult<decimal>> Purchase(int cartId, string userIdFromToken)
        {
            int userId = Convert.ToInt32(userIdFromToken);

            Cart? cart = await context.Carts
                .Include(x => x.Orders)
                .ThenInclude(x => x.Game)
                .FirstOrDefaultAsync(x => x.Id == cartId);

            if (cart == null)
            {
                return OperationResult<decimal>.FailureResult("Cart with such id doesn't exist");
            }
            else if (cart.UserId != userId)
            {
                return OperationResult<decimal>.FailureResult("User ID mismatch");
            }

            decimal totalSum = cart.Orders.Sum(x => x.Game.Price);

            return OperationResult<decimal>.SuccessResult(totalSum);
        }
    }
    public interface ICartRepository
    {
        public Task<OperationResult<IEnumerable<Order>>> GetOrdersFromCartByUserId(int userId);
        public Task<OperationResult<IEnumerable<Order>>> GetOrdersFromCartById(int id);
        public Task<OperationResult<int>> AddToCart(int gameId, int cartId);
        public Task<OperationResult<int>> RemoveFromCart(int gameId, int cartId);
        public Task<OperationResult<decimal>> Purchase(int cartId, string userIdFromToken);
    }
}
