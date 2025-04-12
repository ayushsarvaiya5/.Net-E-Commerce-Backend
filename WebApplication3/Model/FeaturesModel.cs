using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Model
{
    public class FeaturesModel
    {
        [Key, ForeignKey("Mobile")]  // MobileId is both PK and FK
        public int MobileId { get; set; }

        [Required(ErrorMessage = "Ram is Required")]
        public string? Ram { get; set; }

        [Required(ErrorMessage = "Storage is Required")]
        public string? Storage { get; set; }

        [Required(ErrorMessage = "Processor Name is Required")]
        public string? Processor { get; set; }

        [Required(ErrorMessage = "Battery Size is Required")]
        public string? Battery { get; set; }

        [Required(ErrorMessage = "Display Size is Required")]
        public string? DisplaySize { get; set; }

        [Required(ErrorMessage = "Back Camera is Required")]
        public string? BackCamera { get; set; }

        [Required(ErrorMessage = "Front Camera is Required")]
        public string? FrontCamera { get; set; }

        [Required(ErrorMessage = "OS is Required")]
        public string? OS { get; set; }

        [Required(ErrorMessage = "Color is Required")]
        public List<string> Color { get; set; }

        // Navigation Property (One-to-One)
        public MobileModel Mobile { get; set; }
    }
}
