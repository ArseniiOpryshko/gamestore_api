using GameStore.Models.Games;
using GameStore.Models.Orders;
using GameStore.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : Controller
    {
        private ICartRepository repository;
        public CartController(ICartRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet("GetOrdersFromCartById")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersFromCartById(int id)
        {
            var result = await repository.GetOrdersFromCartById(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpGet("GetOrdersFromCartByUserId")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersFromCartByUserId(int id)
        {
            var result = await repository.GetOrdersFromCartByUserId(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpPost("AddToCart")]
        public async Task<ActionResult<int>> AddToCart(int gameId, int cartId)
        {
            var result = await repository.AddToCart(gameId, cartId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpDelete("RemoveFromCart")]
        public async Task<ActionResult<int>> RemoveFromCart(int gameId, int cartId)
        {
            var result = await repository.RemoveFromCart(gameId, cartId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
        [Authorize]
        [HttpPost("Purchase")]     
        public async Task<ActionResult<decimal>> Purchase(int cartId)
        {
            var userClaims = User.Claims;
            var userIdFromToken = userClaims.FirstOrDefault(c => c.Type == "Id")?.Value;

            var result = await repository.Purchase(cartId, userIdFromToken);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
