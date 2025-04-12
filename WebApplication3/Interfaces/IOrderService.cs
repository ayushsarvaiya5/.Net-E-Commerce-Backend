using WebApplication3.DTO;

namespace WebApplication3.Interfaces
{
    public interface IOrderService
    {
        IQueryable<OrderDetailsDto> GetAllOrdersQueryable();
        Task<(List<OrderDetailsDto>, int)> GetOrdersPaginatedAsync(int page, int pageSize, bool includeItems = false);
        Task<OrderDetailsDto> GetOrderByIdAsync(int id, bool includeItems = false);
        Task<OrderDetailsDto> GetOrderByOrderNumberAsync(string orderNumber, bool includeItems = false);
        Task<IEnumerable<OrderDetailsDto>> GetUserOrdersAsync(int userId, bool includeItems = false);
        Task<OrderDetailsDto> CreateOrderAsync(int id, CreateOrderDto orderDto);
        Task UpdateOrderStatusAsync(int id, UpdateOrderStatusDto updateDto);
        Task DeleteOrderAsync(int id);
    }
}
