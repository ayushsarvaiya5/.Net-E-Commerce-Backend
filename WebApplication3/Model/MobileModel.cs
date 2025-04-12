using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Model
{
    public class MobileModel
    {
        [Key]
        [Column("MobileId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Primary Key

        [Required(ErrorMessage = "Mobile Name is Required")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is Required")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Quantity is Required")]
        public int? StockQuantity { get; set; }

        public string? MobileImage { get; set; }

        public int? BrandId { get; set; }

        [ForeignKey("BrandId")]
        public BrandModel Brand { get; set; }

        // One-to-One with Features
        public FeaturesModel Features { get; set; }

        // For Soft Deleting
        public bool IsDeleted { get; set; } = false;
    }
}
