using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.DTO;
using WebApplication3.Interfaces;
using WebApplication3.Model;

namespace WebApplication3.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public IQueryable<OrderDetailsDto> GetAllOrdersQueryable()
        {
            try
            {
                var ordersQuery = _orderRepository.GetAllQueryable();
                return _mapper.ProjectTo<OrderDetailsDto>(ordersQuery);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving order queryable: {ex.Message}", ex);
            }
        }

        public async Task<(List<OrderDetailsDto>, int)> GetOrdersPaginatedAsync(int page, int pageSize, bool includeItems = false)
        {
            try
            {
                if (page < 1 || pageSize < 1)
                    throw new ArgumentException("Invalid page or page size values");

                var (orders, totalOrders) = await _orderRepository.GetOrdersPaginatedAsync(page, pageSize, includeItems);
                var ordersDto = _mapper.Map<List<OrderDetailsDto>>(orders);
                return (ordersDto, totalOrders);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving paginated orders: {ex.Message}", ex);
            }
        }

        public async Task<OrderDetailsDto> GetOrderByIdAsync(int id, bool includeItems = false)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid order ID");

                var order = await _orderRepository.GetByIdAsync(id, includeItems);
                return _mapper.Map<OrderDetailsDto>(order);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving order with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<OrderDetailsDto> GetOrderByOrderNumberAsync(string orderNumber, bool includeItems = false)
        {
            try
            {
                if (string.IsNullOrEmpty(orderNumber))
                    throw new ArgumentException("Invalid order number");

                var order = await _orderRepository.GetByOrderNumberAsync(orderNumber, includeItems);
                return _mapper.Map<OrderDetailsDto>(order);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving order with number {orderNumber}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<OrderDetailsDto>> GetUserOrdersAsync(int userId, bool includeItems = false)
        {
            try
            {
                if (userId <= 0)
                    throw new ArgumentException("Invalid user ID");

                var orders = await _orderRepository.GetUserOrdersAsync(userId, includeItems);
                return _mapper.Map<IEnumerable<OrderDetailsDto>>(orders);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving orders for user with ID {userId}: {ex.Message}", ex);
            }
        }

        public async Task<OrderDetailsDto> CreateOrderAsync(int id, CreateOrderDto orderDto)
        {
            try
            {
                if (orderDto == null)
                    throw new ArgumentNullException(nameof(orderDto), "Order data cannot be null");

                if (orderDto.OrderItems == null || !orderDto.OrderItems.Any())
                    throw new ArgumentException("Order must contain at least one item");

                var order = _mapper.Map<OrderModel>(orderDto);

                order.UserId = id;
                order.OrderDate = DateTime.UtcNow;
                order.Status = OrderStatus.Pending;

                var addedOrder = await _orderRepository.AddAsync(order);

                var completeOrder = await _orderRepository.GetByIdAsync(addedOrder.Id, true);

                return _mapper.Map<OrderDetailsDto>(completeOrder);
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating order: {ex.Message}", ex);
            }
        }

        public async Task UpdateOrderStatusAsync(int id, UpdateOrderStatusDto updateDto)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid order ID");
                if (updateDto == null)
                    throw new ArgumentNullException(nameof(updateDto), "Update data cannot be null");

                await _orderRepository.UpdateStatusAsync(id, updateDto.Status);
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating status for order with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task DeleteOrderAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid order ID");

                await _orderRepository.DeleteAsync(id);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting order with ID {id}: {ex.Message}", ex);
            }
        }
    }
}