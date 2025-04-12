using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Interfaces;
using WebApplication3.Model;

namespace WebApplication3.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly MobileDbContext _db;
        public CartRepository(MobileDbContext db)
        {
            _db = db;
        }

        public async Task AddToCartAsync(CartModel cartItem)
        {
            var existing = await _db.Cart
                .FirstOrDefaultAsync(c => c.UserId == cartItem.UserId && c.MobileId == cartItem.MobileId);

            if (existing != null)
            {
                existing.Quantity += cartItem.Quantity;
                _db.Cart.Update(existing);
            }
            else
            {
                await _db.Cart.AddAsync(cartItem);
            }

            await _db.SaveChangesAsync();
        }

        public async Task<List<CartModel>> GetCartByUserIdAsync(int userId)
        {
            return await _db.Cart
                .Include(c => c.Mobile)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<CartModel?> GetCartItemByUserAndMobileIdAsync(int userId, int mobileId)
        {
            return await _db.Cart
                .FirstOrDefaultAsync(c => c.UserId == userId && c.MobileId == mobileId);
        }

        public async Task RemoveCartItemAsync(CartModel cartItem)
        {
            _db.Cart.Remove(cartItem);
            await _db.SaveChangesAsync();
        }

        public async Task ClearCartAsync(int userId)
        {
            var cartItems = await _db.Cart
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (cartItems.Any())
            {
                _db.Cart.RemoveRange(cartItems);
                await _db.SaveChangesAsync();
            }
        }
    }
}
