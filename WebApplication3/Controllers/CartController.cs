using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTO;
using WebApplication3.Interfaces;
using WebApplication3.Services;
using WebApplication3.Utils;

namespace WebApplication3.Controllers
{
    [Route("api/Cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        [HttpPost("Add")]
        public async Task<IActionResult> AddToCart([FromBody] CartAddDTO dto)
        {
            var user = HttpContext.Items["User"] as UserResponseDTO;
            if (user == null)
            {
                return Unauthorized(new ApiError(401, "Invalid token"));
            }

            try
            {
                await _cartService.AddToCartAsync(user.Id, dto);
                return Ok(new ApiResponse<string>(200, "Item added to cart successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        [Authorize]
        [HttpGet("Get-Cart")]
        public async Task<IActionResult> GetCart()
        {
            var user = HttpContext.Items["User"] as UserResponseDTO;
            if (user == null)
            {
                return Unauthorized(new ApiError(401, "Invalid token"));
            }

            var userId = user.Id;
            
            try
            {
                var cartItems = await _cartService.GetCartByUserIdAsync(userId);
                return Ok(new ApiResponse<List<CartResponseDTO>>(200, cartItems, "Cart fetched successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        [Authorize]
        [HttpDelete("Delete/{mobileId}")]
        public async Task<IActionResult> DeleteCartItem(int mobileId)
        {
            var loggedInUser = HttpContext.Items["User"] as UserResponseDTO;
            if (loggedInUser == null)
                return Unauthorized(new ApiError(401, "Unauthorized"));

            try
            {
                await _cartService.RemoveCartItemAsync(loggedInUser.Id, mobileId);
                return Ok(new ApiResponse<string>(200, "Item removed from cart"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }
    }
}
