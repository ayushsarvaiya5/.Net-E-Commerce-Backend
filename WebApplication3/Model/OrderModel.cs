using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Model
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }

    [Index(nameof(OrderNumber), IsUnique = true)]
    public class OrderModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("OrderId")]
        public int Id { get; set; }

        [Required]
        public string OrderNumber { get; set; } // Unique order reference number

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserModel User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        // Shipping details
        [Required]
        public string ShippingAddress { get; set; }

        [Required]
        public string ShippingCity { get; set; }

        [Required]
        public string ShippingState { get; set; }

        [Required]
        public string ShippingZipCode { get; set; }

        // Payment details
        public string? PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
        public DateTime? PaymentDate { get; set; }

        // Concurrency token for optimistic concurrency control
        [Timestamp]
        public byte[]? RowVersion { get; set; }

        // Navigation property for order items
        public ICollection<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    }

    public class OrderItemModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("OrderItemId")]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public OrderModel Order { get; set; }

        [Required]
        public int MobileId { get; set; }

        [ForeignKey("MobileId")]
        public MobileModel Mobile { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        // Can store the price at the time of order
        public decimal Subtotal { get; set; }
    }
}