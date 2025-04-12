using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Data;
using WebApplication3.Interfaces;
using WebApplication3.Model;

namespace WebApplication3.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MobileDbContext _context;

        public OrderRepository(MobileDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderModel>> GetAllAsync(bool includeItems = false)
        {
            try
            {
                IQueryable<OrderModel> query = _context.Orders.Include(o => o.User);

                if (includeItems)
                    query = query.Include(o => o.OrderItems)
                                 .ThenInclude(oi => oi.Mobile);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving all orders: {ex.Message}", ex);
            }
        }

        public IQueryable<OrderModel> GetAllQueryable()
        {
            try
            {
                IQueryable<OrderModel> query = _context.Orders;
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving order queryable: {ex.Message}", ex);
            }
        }

        public async Task<(List<OrderModel>, int)> GetOrdersPaginatedAsync(int page, int pageSize, bool includeItems = false)
        {
            try
            {
                if (page < 1 || pageSize < 1)
                    throw new ArgumentException("Invalid page or page size values");

                var totalOrders = await _context.Orders.CountAsync();

                IQueryable<OrderModel> query = _context.Orders.Include(o => o.User);

                if (includeItems)
                    query = query.Include(o => o.OrderItems)
                                 .ThenInclude(oi => oi.Mobile);

                var orders = await query
                    .OrderByDescending(o => o.OrderDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (orders, totalOrders);
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

        public async Task<OrderModel> GetByIdAsync(int id, bool includeItems = false)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid order ID");

                IQueryable<OrderModel> query = _context.Orders.Include(o => o.User);

                if (includeItems)
                    query = query.Include(o => o.OrderItems)
                                 .ThenInclude(oi => oi.Mobile);

                var order = await query.FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                    throw new KeyNotFoundException($"Order with ID {id} not found");

                return order;
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

        public async Task<OrderModel> GetByOrderNumberAsync(string orderNumber, bool includeItems = false)
        {
            try
            {
                if (string.IsNullOrEmpty(orderNumber))
                    throw new ArgumentException("Invalid order number");

                IQueryable<OrderModel> query = _context.Orders.Include(o => o.User);

                if (includeItems)
                    query = query.Include(o => o.OrderItems)
                                 .ThenInclude(oi => oi.Mobile);

                var order = await query.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);

                if (order == null)
                    throw new KeyNotFoundException($"Order with number {orderNumber} not found");

                return order;
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

        public async Task<IEnumerable<OrderModel>> GetUserOrdersAsync(int userId, bool includeItems = false)
        {
            try
            {
                if (userId <= 0)
                    throw new ArgumentException("Invalid user ID");

                IQueryable<OrderModel> query = _context.Orders.Where(o => o.UserId == userId).Include(o => o.User);

                if (includeItems)
                    query = query.Include(o => o.OrderItems)
                                 .ThenInclude(oi => oi.Mobile);

                return await query.OrderByDescending(o => o.OrderDate).ToListAsync();
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

        //    public async Task<OrderModel> AddAsync(OrderModel order)
        //    {
        //        using var transaction = await _context.Database.BeginTransactionAsync();
        //        try
        //        {
        //            if (order == null)
        //                throw new ArgumentNullException(nameof(order), "Order cannot be null");

        //            // Generate a unique order number
        //            order.OrderNumber = GenerateOrderNumber();

        //            var total = 0;

        //            //foreach (var item in order.OrderItems)
        //            //{
        //            //    var mobile = await _context.Mobiles.FindAsync(item.MobileId);
        //            //    if (mobile == null)
        //            //        throw new KeyNotFoundException($"Mobile with ID {item.MobileId} not found");

        //            //    if (mobile.StockQuantity < item.Quantity)
        //            //        throw new InvalidOperationException($"Not enough stock for mobile with ID {item.MobileId}. Available: {mobile.StockQuantity}, Requested: {item.Quantity}");

        //            //    total += (int)(item.UnitPrice * item.Quantity);

        //            //    // Update stock quantity
        //            //    mobile.StockQuantity -= item.Quantity;
        //            //    _context.Mobiles.Update(mobile);
        //            //}


        //            // Check if the order items are valid and update stock
        //            foreach (var item in order.OrderItems)
        //            {
        //                var mobile = await _context.Mobiles.FindAsync(item.MobileId);
        //                if (mobile == null)
        //                    throw new KeyNotFoundException($"Mobile with ID {item.MobileId} not found");
        //                if (mobile.StockQuantity < item.Quantity)
        //                    throw new InvalidOperationException($"Not enough stock for mobile with ID {item.MobileId}. Available: {mobile.StockQuantity}, Requested: {item.Quantity}");

        //                // Set the unit price from the mobile entity or from the DTO
        //                //item.UnitPrice = item.UnitPrice > 0 ? item.UnitPrice : mobile.Price;
        //                item.UnitPrice = item.UnitPrice > 0
        //? item.UnitPrice
        //: (mobile.Price.HasValue ? mobile.Price.Value : 0);

        //                item.Subtotal = item.UnitPrice * item.Quantity;

        //                // Update total amount
        //                order.TotalAmount += item.Subtotal;

        //                // Update stock quantity
        //                mobile.StockQuantity -= item.Quantity;
        //                _context.Mobiles.Update(mobile);
        //            }

        //            order.TotalAmount = total;

        //            order.RowVersion = Guid.NewGuid().ToByteArray();

        //            // Add the order
        //            await _context.Orders.AddAsync(order);
        //            await _context.SaveChangesAsync();

        //            await transaction.CommitAsync();
        //            return order;
        //        }
        //        catch (DbUpdateException ex)
        //        {
        //            await transaction.RollbackAsync();
        //            throw new Exception($"Error saving order to database: {ex.Message}", ex);
        //        }
        //        catch (Exception ex)
        //        {
        //            await transaction.RollbackAsync();
        //            throw new Exception($"Error adding order: {ex.Message}", ex);
        //        }
        //    }





        //public async Task<OrderModel> AddAsync(OrderModel order)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        if (order == null)
        //            throw new ArgumentNullException(nameof(order), "Order cannot be null");

        //        // Generate a unique order number
        //        order.OrderNumber = GenerateOrderNumber();

        //        // Initialize total amount using decimal
        //        decimal totalAmount = 0m;

        //        // Check if the order items are valid and update stock
        //        foreach (var item in order.OrderItems)
        //        {
        //            var mobile = await _context.Mobiles.FindAsync(item.MobileId);
        //            if (mobile == null)
        //                throw new KeyNotFoundException($"Mobile with ID {item.MobileId} not found");

        //            if (mobile.StockQuantity < item.Quantity)
        //                throw new InvalidOperationException($"Not enough stock for mobile with ID {item.MobileId}. Available: {mobile.StockQuantity}, Requested: {item.Quantity}");

        //            // Set the unit price from the mobile entity
        //            //item.UnitPrice = mobile.Price;
        //            item.UnitPrice = (decimal)mobile.Price;
        //            item.Subtotal = item.UnitPrice * item.Quantity;

        //            // Add to total amount
        //            totalAmount += item.Subtotal;

        //            // Update stock quantity
        //            mobile.StockQuantity -= item.Quantity;
        //            _context.Mobiles.Update(mobile);
        //        }

        //        // Set the total amount on the order
        //        order.TotalAmount = totalAmount;
        //        order.RowVersion = Guid.NewGuid().ToByteArray();

        //        // Add the order
        //        await _context.Orders.AddAsync(order);
        //        await _context.SaveChangesAsync();
        //        await transaction.CommitAsync();

        //        return order;
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        await transaction.RollbackAsync();
        //        throw new Exception($"Error saving order to database: {ex.Message}", ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        throw new Exception($"Error adding order: {ex.Message}", ex);
        //    }
        //}


        public async Task<OrderModel> AddAsync(OrderModel order)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (order == null)
                    throw new ArgumentNullException(nameof(order), "Order cannot be null");

                // Generate a unique order number
                order.OrderNumber = GenerateOrderNumber();

                // Generate a unique transaction ID
                order.TransactionId = GenerateTransactionId();

                // Set payment method to Online
                order.PaymentMethod = "Online";

                // Set payment date to current UTC time
                order.PaymentDate = DateTime.UtcNow;

                // Initialize total amount using decimal
                decimal totalAmount = 0m;

                // Check if the order items are valid and update stock
                foreach (var item in order.OrderItems)
                {
                    var mobile = await _context.Mobiles.FindAsync(item.MobileId);
                    if (mobile == null)
                        throw new KeyNotFoundException($"Mobile with ID {item.MobileId} not found");
                    if (mobile.StockQuantity < item.Quantity)
                        throw new InvalidOperationException($"Not enough stock for mobile with ID {item.MobileId}. Available: {mobile.StockQuantity}, Requested: {item.Quantity}");

                    // Set the unit price from the mobile entity
                    item.UnitPrice = (decimal)mobile.Price;
                    item.Subtotal = item.UnitPrice * item.Quantity;

                    // Add to total amount
                    totalAmount += item.Subtotal;

                    // Update stock quantity
                    mobile.StockQuantity -= item.Quantity;
                    _context.Mobiles.Update(mobile);
                }

                // Set the total amount on the order
                order.TotalAmount = totalAmount;
                order.RowVersion = Guid.NewGuid().ToByteArray();

                // Add the order
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return order;
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error saving order to database: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error adding order: {ex.Message}", ex);
            }
        }

        private string GenerateOrderNumber()
        {
            // Format: ORD-YYYYMMDD-XXXX (X is a random digit)
            var dateStr = DateTime.Now.ToString("yyyyMMdd");
            var random = new Random();
            var randomPart = random.Next(1000, 9999).ToString();
            return $"ORD-{dateStr}-{randomPart}";
        }

        private string GenerateTransactionId()
        {
            // Format: TXN-YYYYMMDD-XXXX (X is a random digit)
            var dateStr = DateTime.Now.ToString("yyyyMMdd");
            var random = new Random();
            var randomPart = random.Next(1000, 9999).ToString();
            return $"TXN-{dateStr}-{randomPart}";
        }

        public async Task UpdateAsync(OrderModel order)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (order == null)
                    throw new ArgumentNullException(nameof(order), "Order cannot be null");

                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await transaction.RollbackAsync();
                throw new DbUpdateConcurrencyException("The order has been modified by another user. Please refresh and try again.", ex);
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error updating order in database: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error updating order: {ex.Message}", ex);
            }
        }

        //public async Task UpdateStatusAsync(int id, OrderStatus status, byte[] rowVersion)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        if (id <= 0)
        //            throw new ArgumentException("Invalid order ID");

        //        var order = await _context.Orders.FindAsync(id);
        //        if (order == null)
        //            throw new KeyNotFoundException($"Order with ID {id} not found");

        //        // Set concurrency token for optimistic concurrency control
        //        order.RowVersion = rowVersion;
        //        order.Status = status;

        //        _context.Orders.Update(order);
        //        await _context.SaveChangesAsync();
        //        await transaction.CommitAsync();
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        await transaction.RollbackAsync();
        //        throw new DbUpdateConcurrencyException("The order has been modified by another user. Please refresh and try again.", ex);
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        await transaction.RollbackAsync();
        //        throw new Exception($"Error updating order status in database: {ex.Message}", ex);
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        await transaction.RollbackAsync();
        //        throw;
        //    }
        //    catch (ArgumentException)
        //    {
        //        await transaction.RollbackAsync();
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        throw new Exception($"Error updating status for order with ID {id}: {ex.Message}", ex);
        //    }
        //}

        public async Task UpdateStatusAsync(int id, OrderStatus status)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid order ID");

                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                    throw new KeyNotFoundException($"Order with ID {id} not found");

                // Update the status directly using the current version
                order.Status = status;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await transaction.RollbackAsync();
                throw new DbUpdateConcurrencyException("The order has been modified by another user. Please refresh and try again.", ex);
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error updating order status in database: {ex.Message}", ex);
            }
            catch (KeyNotFoundException)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (ArgumentException)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error updating status for order with ID {id}: {ex.Message}", ex);
            }
        }

        //public async Task DeleteAsync(int id, byte[] rowVersion)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        if (id <= 0)
        //            throw new ArgumentException("Invalid order ID");

        //        var order = await _context.Orders
        //            .Include(o => o.OrderItems)
        //            .FirstOrDefaultAsync(o => o.Id == id);

        //        if (order == null)
        //            throw new KeyNotFoundException($"Order with ID {id} not found");

        //        // Set concurrency token for optimistic concurrency control
        //        order.RowVersion = rowVersion;

        //        // For orders that are not delivered yet, restore stock quantities
        //        if (order.Status != OrderStatus.Delivered)
        //        {
        //            foreach (var item in order.OrderItems)
        //            {
        //                var mobile = await _context.Mobiles.FindAsync(item.MobileId);
        //                if (mobile != null)
        //                {
        //                    mobile.StockQuantity += item.Quantity;
        //                    _context.Mobiles.Update(mobile);
        //                }
        //            }
        //        }

        //        _context.Orders.Remove(order);
        //        await _context.SaveChangesAsync();
        //        await transaction.CommitAsync();
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        await transaction.RollbackAsync();
        //        throw new DbUpdateConcurrencyException("The order has been modified by another user. Please refresh and try again.", ex);
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        await transaction.RollbackAsync();
        //        throw new Exception($"Error deleting order from database: {ex.Message}", ex);
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        await transaction.RollbackAsync();
        //        throw;
        //    }
        //    catch (ArgumentException)
        //    {
        //        await transaction.RollbackAsync();
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        throw new Exception($"Error deleting order with ID {id}: {ex.Message}", ex);
        //    }
        //}

        public async Task DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid order ID");

                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                    throw new KeyNotFoundException($"Order with ID {id} not found");

                // No need to set concurrency token - using current version from database

                // For orders that are not delivered yet, restore stock quantities
                if (order.Status != OrderStatus.Delivered)
                {
                    foreach (var item in order.OrderItems)
                    {
                        var mobile = await _context.Mobiles.FindAsync(item.MobileId);
                        if (mobile != null)
                        {
                            mobile.StockQuantity += item.Quantity;
                            _context.Mobiles.Update(mobile);
                        }
                    }
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await transaction.RollbackAsync();
                throw new DbUpdateConcurrencyException("The order has been modified by another user. Please refresh and try again.", ex);
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error deleting order from database: {ex.Message}", ex);
            }
            catch (KeyNotFoundException)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (ArgumentException)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error deleting order with ID {id}: {ex.Message}", ex);
            }
        }

        //private string GenerateOrderNumber()
        //{
        //    // Format: ORD-YYYYMMDD-XXXX (X is a random digit)
        //    var dateStr = DateTime.Now.ToString("yyyyMMdd");
        //    var random = new Random();
        //    var randomPart = random.Next(1000, 9999).ToString();
        //    return $"ORD-{dateStr}-{randomPart}";
        //}
    }
}