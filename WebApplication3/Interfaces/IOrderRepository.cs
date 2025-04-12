using WebApplication3.Model;

namespace WebApplication3.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderModel>> GetAllAsync(bool includeItems = false);
        IQueryable<OrderModel> GetAllQueryable();
        Task<(List<OrderModel>, int)> GetOrdersPaginatedAsync(int page, int pageSize, bool includeItems = false);
        Task<OrderModel> GetByIdAsync(int id, bool includeItems = false);
        Task<OrderModel> GetByOrderNumberAsync(string orderNumber, bool includeItems = false);
        Task<IEnumerable<OrderModel>> GetUserOrdersAsync(int userId, bool includeItems = false);
        Task<OrderModel> AddAsync(OrderModel order);
        Task UpdateAsync(OrderModel order);
        Task UpdateStatusAsync(int id, OrderStatus status);
        Task DeleteAsync(int id);
    }
}
