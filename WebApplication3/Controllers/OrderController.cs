//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.OData.Query;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using WebApplication3.DTO;
//using WebApplication3.Interfaces;
//using WebApplication3.Model;
//using WebApplication3.Utils;

//namespace WebApplication3.Controllers
//{
//    [Route("api/Order")]
//    [ApiController]
//    public class OrderController : ControllerBase
//    {
//        private readonly IOrderService _orderService;

//        public OrderController(IOrderService orderService)
//        {
//            _orderService = orderService;
//        }

//        [Authorize]
//        [HttpPost("Create-Order")]
//        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
//        {
//            try
//            {
//                if (orderDto == null)
//                {
//                    return BadRequest(new ApiError(400, "Invalid order data"));
//                }

//                // Create the order through the service
//                var createdOrder = await _orderService.CreateOrderAsync(orderDto);
//                return Ok(new ApiResponse<OrderDetailsDto>(201, createdOrder, "Order created successfully"));
//            }
//            catch (ArgumentNullException ex)
//            {
//                return BadRequest(new ApiError(400, ex.Message));
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(new ApiError(400, ex.Message));
//            }
//            catch (KeyNotFoundException ex)
//            {
//                return NotFound(new ApiError(404, ex.Message));
//            }
//            catch (InvalidOperationException ex)
//            {
//                return BadRequest(new ApiError(400, ex.Message));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new ApiError(400, ex.Message));
//            }
//        }

//        [Authorize(Roles = "Admin, Employee")]
//        [EnableQuery]
//        [HttpGet("Get-All-Orders")]
//        public IActionResult GetAllOrders()
//        {
//            try
//            {
//                var orders = _orderService.GetAllOrdersQueryable();
//                return Ok(orders);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new ApiError(400, ex.Message));
//            }
//        }

//        [Authorize(Roles = "Admin, Employee")]
//        [HttpGet("Get-Orders-Paginated")]
//        public async Task<IActionResult> GetOrdersPaginated(int page = 1, int pageSize = 10, bool includeItems = false)
//        {
//            try
//            {
//                var (orders, totalOrders) = await _orderService.GetOrdersPaginatedAsync(page, pageSize, includeItems);

//                var response = new OrdersPaginationResponse
//                {
//                    Page = page,
//                    PageSize = pageSize,
//                    TotalOrders = totalOrders,
//                    TotalPages = (int)Math.Ceiling(totalOrders / (double)pageSize),
//                    Orders = orders
//                };

//                return Ok(new ApiResponse<OrdersPaginationResponse>(200, response, "Orders pagination successful"));
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(new ApiError(400, ex.Message));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new ApiError(400, ex.Message));
//            }
//        }

//        [Authorize]
//        [HttpGet("Get-Order-By-Id/{id}")]
//        public async Task<IActionResult> GetOrderById(int id, [FromQuery] bool includeItems = true)
//        {
//            try
//            {
//                var order = await _orderService.GetOrderByIdAsync(id, includeItems);
//                return Ok(new ApiResponse<OrderDetailsDto>(200, order, "Order fetched successfully"));
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(new ApiError(400, ex.Message));
//            }
//            catch (KeyNotFoundException ex)
//            {
//                return NotFound(new ApiError(404, ex.Message));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new ApiError(400, ex.Message));
//            }
//        }

//        [Authorize]
//        [HttpGet("Get-Order-By-Number/{orderNumber}")]
//        public async Task<IActionResult> GetOrderByNumber(string orderNumber, [FromQuery] bool includeItems = true)
//        {
//            try
//            {
//                var order = await _orderService.GetOrderByOrderNumberAsync(orderNumber, includeItems);
//                return Ok(new ApiResponse<OrderDetailsDto>(200, order, "Order fetched successfully"));
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(new ApiError(400, ex.Message));
//            }
//            catch (KeyNotFoundException ex)
//            {
//                return NotFound(new ApiError(404, ex.Message));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new ApiError(400, ex.Message));
//            }
//        }

//        [Authorize]
//        [HttpGet("Get-User-Orders/{userId}")]
//        public async Task<IActionResult> GetUserOrders(int userId, [FromQuery] bool includeItems = true)
//        {
//            try
//            {
//                var orders = await _orderService.GetUserOrdersAsync(userId, includeItems);
//                return Ok(new ApiResponse<IEnumerable<OrderDetailsDto>>(200, orders, "User orders fetched successfully"));
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(new ApiError(400, ex.Message));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new ApiError(400, ex.Message));
//            }
//        }

//        [Authorize(Roles = "Admin, Employee")]
//        [HttpPatch("Update-Order-Status/{id}")]
//        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto updateDto)
//        {
//            try
//            {
//                if (id <= 0)
//                {
//                    return BadRequest(new ApiError(400, "Invalid order ID"));
//                }

//                if (updateDto == null)
//                {
//                    return BadRequest(new ApiError(400, "Invalid update data"));
//                }

//                await _orderService.UpdateOrderStatusAsync(id, updateDto);
//                return Ok(new ApiResponse<string>(200, "Order status updated successfully"));
//            }
//            catch (ArgumentNullException ex)
//            {
//                return BadRequest(new ApiError(400, ex.Message));
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(new ApiError(400, ex.Message));
//            }
//            catch (KeyNotFoundException ex)
//            {
//                return NotFound(new ApiError(404, ex.Message));
//            }
//            catch (DbUpdateConcurrencyException ex)
//            {
//                return Conflict(new ApiError(409, "The order has been modified by another user. Please refresh and try again."));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new ApiError(400, ex.Message));
//            }
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpDelete("Delete-Order/{id}")]
//        public async Task<IActionResult> DeleteOrder(int id, [FromQuery] string rowVersion)
//        {
//            try
//            {
//                if (id <= 0)
//                {
//                    return BadRequest(new ApiError(400, "Invalid order ID"));
//                }

//                if (string.IsNullOrEmpty(rowVersion))
//                {
//                    return BadRequest(new ApiError(400, "Row version is required for concurrency check"));
//                }

//                await _orderService.DeleteOrderAsync(id, rowVersion);
//                return Ok(new ApiResponse<string>(200, "Order deleted successfully"));
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(new ApiError(400, ex.Message));
//            }
//            catch (KeyNotFoundException ex)
//            {
//                return NotFound(new ApiError(404, ex.Message));
//            }
//            catch (DbUpdateConcurrencyException ex)
//            {
//                return Conflict(new ApiError(409, "The order has been modified by another user. Please refresh and try again."));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new ApiError(400, ex.Message));
//            }
//        }
//    }
//}







using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication3.DTO;
using WebApplication3.Interfaces;
using WebApplication3.Model;
using WebApplication3.Utils;

namespace WebApplication3.Controllers
{
    [Route("api/Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, ICartService cartService, IMapper mapper)
        {
            _orderService = orderService;
            _cartService = cartService;
            _mapper = mapper;
        }

        //[Authorize]
        //[HttpPost("Create-Order")]
        //public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        //{
        //    try
        //    {
        //        if (orderDto == null)
        //        {
        //            return BadRequest(new ApiError(400, "Invalid order data"));
        //        }

        //        var user = HttpContext.Items["User"] as UserResponseDTO;
        //        if (user == null)
        //        {
        //            return Unauthorized(new ApiError(401, "Invalid token"));
        //        }

        //        // Get cart items for the user
        //        var cartItems = await _cartService.GetCartByUserIdAsync(user.Id);

        //        if (cartItems == null || cartItems.Count == 0)
        //        {
        //            return BadRequest(new ApiError(400, "Cart is empty. Add items to cart before placing an order."));
        //        }

        //        // Map cart items to order items
        //        orderDto.OrderItems = cartItems.Select(cart => new CreateOrderItemDto
        //        {
        //            MobileId = cart.MobileId,
        //            Quantity = cart.Quantity,
        //            Price = cart.Price
        //        }).ToList();

        //        // Create the order through the service
        //        var createdOrder = await _orderService.CreateOrderAsync(user.Id, orderDto);

        //        // Clear the user's cart after successful order creation
        //        await _cartService.ClearCartAsync(user.Id);

        //        return Ok(new ApiResponse<OrderDetailsDto>(201, createdOrder, "Order created successfully"));
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(new ApiError(400, ex.Message));
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(new ApiError(400, ex.Message));
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new ApiError(404, ex.Message));
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return BadRequest(new ApiError(400, ex.Message));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ApiError(400, ex.Message));
        //    }
        //}

        [Authorize]
        [HttpPost("Create-Order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDtoFirst orderDtoFirst)
        {
            try
            {
                if (orderDtoFirst == null)
                {
                    return BadRequest(new ApiError(400, "Invalid order data"));
                }

                var user = HttpContext.Items["User"] as UserResponseDTO;
                if (user == null)
                {
                    return Unauthorized(new ApiError(401, "Invalid token"));
                }

                var orderDto = _mapper.Map<CreateOrderDto>(orderDtoFirst);

                // Get cart items for the user
                var cartItems = await _cartService.GetCartByUserIdAsync(user.Id);
                if (cartItems == null || cartItems.Count == 0)
                {
                    return BadRequest(new ApiError(400, "Cart is empty. Add items to cart before placing an order."));
                }

                // Map cart items to order items
                orderDto.OrderItems = cartItems.Select(cart => new CreateOrderItemDto
                {
                    MobileId = cart.MobileId,
                    Quantity = cart.Quantity,
                    Price = cart.Price // Note: This Price will be replaced with the actual price from the Mobile entity in the repository
                }).ToList();

                // Create the order through the service
                var createdOrder = await _orderService.CreateOrderAsync(user.Id, orderDto);

                // Clear the user's cart after successful order creation
                await _cartService.ClearCartAsync(user.Id);

                return Ok(new ApiResponse<OrderDetailsDto>(201, createdOrder, "Order created successfully"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiError(404, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        [Authorize(Roles = "Admin, Employee")]
        [EnableQuery]
        [HttpGet("Get-All-Orders")]
        public IActionResult GetAllOrders()
        {
            try
            {
                var orders = _orderService.GetAllOrdersQueryable();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        [Authorize(Roles = "Admin, Employee")]
        [HttpGet("Get-Orders-Paginated")]
        public async Task<IActionResult> GetOrdersPaginated(int page = 1, int pageSize = 10, bool includeItems = false)
        {
            try
            {
                var (orders, totalOrders) = await _orderService.GetOrdersPaginatedAsync(page, pageSize, includeItems);

                var response = new OrdersPaginationResponse
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalOrders = totalOrders,
                    TotalPages = (int)Math.Ceiling(totalOrders / (double)pageSize),
                    Orders = orders
                };

                return Ok(new ApiResponse<OrdersPaginationResponse>(200, response, "Orders pagination successful"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        [Authorize]
        [HttpGet("Get-Order-By-Id/{id}")]
        public async Task<IActionResult> GetOrderById(int id, [FromQuery] bool includeItems = true)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id, includeItems);
                return Ok(new ApiResponse<OrderDetailsDto>(200, order, "Order fetched successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiError(404, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        [Authorize]
        [HttpGet("Get-Order-By-Number/{orderNumber}")]
        public async Task<IActionResult> GetOrderByNumber(string orderNumber, [FromQuery] bool includeItems = true)
        {
            try
            {
                var order = await _orderService.GetOrderByOrderNumberAsync(orderNumber, includeItems);
                return Ok(new ApiResponse<OrderDetailsDto>(200, order, "Order fetched successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiError(404, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        [Authorize]
        [HttpGet("Get-User-Orders/{userId}")]
        public async Task<IActionResult> GetUserOrders(int userId, [FromQuery] bool includeItems = true)
        {
            try
            {
                var orders = await _orderService.GetUserOrdersAsync(userId, includeItems);
                return Ok(new ApiResponse<IEnumerable<OrderDetailsDto>>(200, orders, "User orders fetched successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        //[Authorize(Roles = "Admin, Employee")]
        //[HttpPatch("Update-Order-Status/{id}")]
        //public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto updateDto)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //        {
        //            return BadRequest(new ApiError(400, "Invalid order ID"));
        //        }

        //        if (updateDto == null)
        //        {
        //            return BadRequest(new ApiError(400, "Invalid update data"));
        //        }

        //        await _orderService.UpdateOrderStatusAsync(id, updateDto);
        //        return Ok(new ApiResponse<string>(200, "Order status updated successfully"));
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(new ApiError(400, ex.Message));
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(new ApiError(400, ex.Message));
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new ApiError(404, ex.Message));
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        return Conflict(new ApiError(409, "The order has been modified by another user. Please refresh and try again."));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ApiError(400, ex.Message));
        //    }
        //}

        [Authorize(Roles = "Admin, Employee")]
        [HttpPatch("Update-Order-Status/{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto updateDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiError(400, "Invalid order ID"));
                }
                if (updateDto == null)
                {
                    return BadRequest(new ApiError(400, "Invalid update data"));
                }
                await _orderService.UpdateOrderStatusAsync(id, updateDto);
                return Ok(new ApiResponse<string>(200, "Order status updated successfully"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiError(404, ex.Message));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(new ApiError(409, "The order has been modified by another user. Please refresh and try again."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        //[Authorize(Roles = "Admin")]
        //[HttpDelete("Delete-Order/{id}")]
        //public async Task<IActionResult> DeleteOrder(int id, [FromQuery] string rowVersion)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //        {
        //            return BadRequest(new ApiError(400, "Invalid order ID"));
        //        }

        //        if (string.IsNullOrEmpty(rowVersion))
        //        {
        //            return BadRequest(new ApiError(400, "Row version is required for concurrency check"));
        //        }

        //        await _orderService.DeleteOrderAsync(id, rowVersion);
        //        return Ok(new ApiResponse<string>(200, "Order deleted successfully"));
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(new ApiError(400, ex.Message));
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new ApiError(404, ex.Message));
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        return Conflict(new ApiError(409, "The order has been modified by another user. Please refresh and try again."));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ApiError(400, ex.Message));
        //    }
        //}

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete-Order/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiError(400, "Invalid order ID"));
                }
                await _orderService.DeleteOrderAsync(id);
                return Ok(new ApiResponse<string>(200, "Order deleted successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiError(404, ex.Message));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(new ApiError(409, "The order has been modified by another user. Please refresh and try again."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }
    }
}