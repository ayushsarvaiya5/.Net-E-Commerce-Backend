using WebApplication3.DTO;

namespace WebApplication3.Interfaces
{
    public interface ICartService
    {
        public Task AddToCartAsync(int id, CartAddDTO dto);

        public Task<List<CartResponseDTO>> GetCartByUserIdAsync(int userId);

        public Task RemoveCartItemAsync(int userId, int mobileId);

        Task ClearCartAsync(int userId);
    }
}
