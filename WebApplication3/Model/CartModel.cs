using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Model
{
    public class CartModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserModel User { get; set; }

        [Required]
        public int MobileId { get; set; }

        [ForeignKey("MobileId")]
        public MobileModel Mobile { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
