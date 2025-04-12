using System.Threading.Tasks;
using WebApplication3.Model;

namespace WebApplication3.Interfaces
{
    public interface ICartRepository
    {
        Task AddToCartAsync(CartModel cartItem);
        Task<List<CartModel>> GetCartByUserIdAsync(int userId);
        Task<CartModel?> GetCartItemByUserAndMobileIdAsync(int userId, int mobileId);
        Task RemoveCartItemAsync(CartModel cartItem);
        Task ClearCartAsync(int userId);
    }
}
