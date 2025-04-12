using System.ComponentModel.DataAnnotations;

namespace WebApplication3.DTO
{
    //public class CreateMobileDto
    //{
    //    [Required]
    //    public string Name { get; set; }

    //    [Required]
    //    public decimal Price { get; set; }

    //    [Required]
    //    public int StockQuantity { get; set; }

    //    [Required]
    //    public int BrandId { get; set; }  // Reference brand by ID

    //    public CreateFeaturesDto Features { get; set; }  // Required while creating a new mobile
    //}

    //public class MobileResponse_Dto
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public decimal Price { get; set; }
    //    public int StockQuantity { get; set; }
    //    public string BrandName { get; set; }  // Include Brand Name instead of Brand object
    //    public FeaturesResponse_Dto Features { get; set; }  // Nested DTO for features
    //}

    //public class CreateBrandDto
    //{
    //    [Required]
    //    public string Name { get; set; }

    //    [Required]
    //    public string Country { get; set; }

    //    public string Description { get; set; }
    //    public int FoundedYear { get; set; }

    //    [Url]
    //    public string Website { get; set; }
    //}


    //public class BrandReponse_Dto
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string Country { get; set; }
    //}

    //public class CreateFeaturesDto
    //{
    //    [Required]
    //    public string Ram { get; set; }

    //    [Required]
    //    public string Storage { get; set; }

    //    [Required]
    //    public string Processor { get; set; }

    //    [Required]
    //    public string Battery { get; set; }

    //    [Required]
    //    public string DisplaySize { get; set; }

    //    [Required]
    //    public string BackCamera { get; set; }

    //    [Required]
    //    public string FrontCamera { get; set; }

    //    [Required]
    //    public string OS { get; set; }

    //    [Required]
    //    public List<string> Color { get; set; }
    //}

    //public class FeaturesResponse_Dto
    //{
    //    public string Ram { get; set; }
    //    public string Storage { get; set; }
    //    public string Processor { get; set; }
    //    public string Battery { get; set; }
    //    public string DisplaySize { get; set; }
    //    public string BackCamera { get; set; }
    //    public string FrontCamera { get; set; }
    //    public string OS { get; set; }
    //    public List<string> Color { get; set; }
    //}

















    public class MobileCreateDTO
    {
        [Required(ErrorMessage = "Mobile Name is Required")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is Required")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Quantity is Required")]
        public int? StockQuantity { get; set; }

        [Required(ErrorMessage = "BrandId is Required")]
        public int? BrandId { get; set; }

        [Required(ErrorMessage = "Image is Required")]
        public string? MobileImage { get; set; }
    }

    public class MobileResponseDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? StockQuantity { get; set; }
        public string? MobileImage { get; set; }
        public BrandResponseDTO? Brand { get; set; }
    }

    public class FeaturesCreateDTO
    {
        [Required(ErrorMessage = "MobileId is required")]
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
    }

    public class FeaturesResponseDTO
    {
        public int MobileId { get; set; }
        public string? Ram { get; set; }
        public string? Storage { get; set; }
        public string? Processor { get; set; }
        public string? Battery { get; set; }
        public string? DisplaySize { get; set; }
        public string? BackCamera { get; set; }
        public string? FrontCamera { get; set; }
        public string? OS { get; set; }
        public List<string>? Color { get; set; }
    }

    public class BrandCreateDTO
    {
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
    }

    public class BrandResponseDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; }
        public int? FoundedYear { get; set; }
        public string? Website { get; set; }
    }










    public class MobileDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? StockQuantity { get; set; }
        public string? MobileImage { get; set; }
        public string? BrandName { get; set; } // Include Brand Name
    }

    public class MobileDetailsDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? StockQuantity { get; set; }
        public string? MobileImage { get; set; }
        public string? BrandName { get; set; }

        public FeaturesCreateDTO? Features { get; set; } // Include Features
    }

    public class CreateMobileRequestDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? StockQuantity { get; set; }
        public int? BrandId { get; set; }
        public IFormFile? ImageFile { get; set; }
    }

    public class CreateMobileDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? StockQuantity { get; set; }
        public string? MobileImage { get; set; }
        public int? BrandId { get; set; }
    }

    public class UpdateMobileDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? StockQuantity { get; set; }
        public int? BrandId { get; set; }

    }
}
