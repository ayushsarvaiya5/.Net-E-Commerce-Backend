using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Model
{
    public class BrandModel
    {
        [Key]
        [Column("BrandId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Brand name is Required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Brand country origin is Required")]
        public string? Country { get; set; }

        [MaxLength(255)]
        [Required(ErrorMessage = "Brand description is Required")]
        public string? Description { get; set; }

        [RegularExpression("^(17|20)\\d{2}$\r\n")]
        [Required(ErrorMessage = "Brand Founded Year is Required")]
        public int? FoundedYear { get; set; }

        [Url]
        [Required(ErrorMessage = "Brand Website is Required")]
        public string? Website { get; set; }

        // Navigation Property
        public List<MobileModel> Mobiles { get; set; }
    }
}
