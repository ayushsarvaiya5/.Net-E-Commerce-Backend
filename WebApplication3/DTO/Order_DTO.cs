using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using WebApplication3.Model;

namespace WebApplication3.DTO
{
    public class CreateOrderDtoFirst
    {

        [Required(ErrorMessage = "Shipping address is required")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Shipping city is required")]
        public string ShippingCity { get; set; }

        [Required(ErrorMessage = "Shipping state is required")]
        public string ShippingState { get; set; }

        [Required(ErrorMessage = "Shipping zip code is required")]
        public string ShippingZipCode { get; set; }

    }

    public class CreateOrderDto
    {

        [Required(ErrorMessage = "Shipping address is required")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Shipping city is required")]
        public string ShippingCity { get; set; }

        [Required(ErrorMessage = "Shipping state is required")]
        public string ShippingState { get; set; }

        [Required(ErrorMessage = "Shipping zip code is required")]
        public string ShippingZipCode { get; set; }

        public string? PaymentMethod { get; set; }

        // Making OrderItems optional since we'll fetch them from the cart
        public List<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>();
    }

    public class CreateOrderItemDto
    {
        [Required(ErrorMessage = "Mobile ID is required")]
        public int MobileId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }
    }

    // DTOs for responses
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingZipCode { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }

    public class OrderItemDto
    {
        public int Id { get; set; }
        public int MobileId { get; set; }
        public string MobileName { get; set; }
        public string MobileImage { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }

    // DTOs for updating an order status
    public class UpdateOrderStatusDto
    {
        [Required(ErrorMessage = "Status is required")]
        [EnumDataType(typeof(OrderStatus), ErrorMessage = "Invalid order status")]
        public OrderStatus Status { get; set; }

    }

    // DTOs for order pagination
    public class OrdersPaginationResponse
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalOrders { get; set; }
        public int TotalPages { get; set; }
        public List<OrderDetailsDto> Orders { get; set; } = new List<OrderDetailsDto>();
    }
}