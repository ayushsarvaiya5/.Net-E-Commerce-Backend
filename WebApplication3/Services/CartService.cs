using AutoMapper;
using WebApplication3.DTO;
using WebApplication3.Interfaces;
using WebApplication3.Model;

namespace WebApplication3.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepo, IMapper mapper)
        {
            _cartRepo = cartRepo;
            _mapper = mapper;
        }

        public async Task AddToCartAsync(int id, CartAddDTO dto)
        {
            if (dto.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            var cartItem = new CartModel
            {
                UserId = id,
                MobileId = dto.MobileId,
                Quantity = dto.Quantity
            };

            await _cartRepo.AddToCartAsync(cartItem);
        }

        public async Task<List<CartResponseDTO>> GetCartByUserIdAsync(int userId)
        {
            var cartItems = await _cartRepo.GetCartByUserIdAsync(userId);
            return cartItems.Select(c => new CartResponseDTO
            {
                Id = c.Id,
                UserId = c.UserId,
                MobileId = c.MobileId,
                Quantity = c.Quantity,
                MobileName = c.Mobile?.Name ?? "",
                Price = c.Mobile?.Price ?? 0
            }).ToList();
        }

        public async Task RemoveCartItemAsync(int userId, int mobileId)
        {
            var cartItem = await _cartRepo.GetCartItemByUserAndMobileIdAsync(userId, mobileId);
            if (cartItem == null)
                throw new KeyNotFoundException("Cart item not found.");

            await _cartRepo.RemoveCartItemAsync(cartItem);
        }

        public async Task ClearCartAsync(int userId)
        {
            await _cartRepo.ClearCartAsync(userId);
        }
    }
}
