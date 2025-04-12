//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using WebApplication3.Data;
//using WebApplication3.DTO;
//using WebApplication3.Model;
//using WebApplication3.Utils;

//namespace WebApplication3.Controllers
//{
//    [Route("api/Mobile")]
//    [ApiController]
//    public class MobileController : ControllerBase
//    {
//        private readonly MobileDbContext _db;

//        public MobileController(MobileDbContext db)
//        {
//            _db = db;
//        }

//        [Authorize]
//        [HttpGet("Get-AllMobiles")]
//        public async Task<IActionResult> GetAllMobiles()
//        {
//            var data = await _db.Mobiles.ToListAsync();
//            return Ok(new ApiResponse<List<MobileModel>>(200, data, "Mobile Fetched Successfully"));
//        }

//        [Authorize(Roles = "Admin, Employee")]
//        [HttpPost("Add-Mobile")]
//        public async Task<IActionResult> AddMobile(CreateMobileDto mobile)
//        {
//            //await _db.Mobiles.AddAsync(mobile);
//            return Ok(new ApiResponse<String>(201, "Mobile Added Successfully"));
//        }


//    }
//}
















////Image upload Cloudinory

//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.IO;
//using System.Threading.Tasks;
//using WebApplication3.Utils;

//[Route("api/[controller]")]
//[ApiController]
//public class MobileController : ControllerBase
//{
//    private readonly CloudinaryService _cloudinaryService;
//    private readonly IWebHostEnvironment _environment;

//    public MobileController(CloudinaryService cloudinaryService, IWebHostEnvironment environment)
//    {
//        _cloudinaryService = cloudinaryService;
//        _environment = environment;
//    }

//    [HttpPost("UploadMobileImage")]
//    public async Task<IActionResult> UploadMobileImage(IFormFile imageFile)
//    {
//        if (imageFile == null || imageFile.Length == 0)
//        {
//            return BadRequest("No image provided.");
//        }

//        // Step 1: Save to local folder
//        string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

//        if (!Directory.Exists(uploadsFolder))
//        {
//            Directory.CreateDirectory(uploadsFolder);
//        }

//        string localFilePath = Path.Combine(uploadsFolder, imageFile.FileName);

//        using (var stream = new FileStream(localFilePath, FileMode.Create))
//        {
//            await imageFile.CopyToAsync(stream);
//        }

//        // Step 2: Upload to Cloudinary
//        string? cloudinaryUrl = await _cloudinaryService.UploadImageAsync(localFilePath);

//        if (cloudinaryUrl == null)
//        {
//            return StatusCode(500, "Image upload to Cloudinary failed. Please try again.");
//        }

//        return Ok(new { ImageUrl = cloudinaryUrl });
//    }
//}



























//using Microsoft.AspNetCore.Mvc;
//using WebApplication3.DTO;
//using WebApplication3.Services;
//using Microsoft.AspNetCore.Authorization;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using WebApplication3.Utils;
//using WebApplication3.Model;

//namespace WebApplication3.Controllers
//{
//    [Route("api/Mobile")]
//    [ApiController]
//    public class MobileController : ControllerBase
//    {
//        private readonly MobileService _mobileService;

//        public MobileController(MobileService mobileService)
//        {
//            _mobileService = mobileService;
//        }

//        [Authorize(Roles = "Admin, Employee")]
//        [HttpPost("Create")]
//        public async Task<IActionResult> CreateMobile([FromBody] CreateMobileDto mobileDto)
//        {
//            if (mobileDto == null)
//            {
//                return BadRequest(new { Message = "Invalid mobile data" });
//            }

//            await _mobileService.AddMobileAsync(mobileDto);
//            return Ok(new ApiResponse<string>(201, "Mobile created successfully"));
//        }

//        [AllowAnonymous]
//        [HttpGet("Get-All")]
//        public async Task<IActionResult> GetAllMobiles([FromQuery] bool includeBrand = false)
//        {
//            var mobiles = await _mobileService.GetAllMobilesAsync(includeBrand);
//            return Ok(new { Data = mobiles, Message = "Fetched all mobiles successfully" });
//        }

//        [AllowAnonymous]
//        [HttpGet("Get/{id}")]
//        public async Task<IActionResult> GetMobileById(int id, [FromQuery] bool includeFeatures = false, [FromQuery] bool includeBrand = false)
//        {
//            var mobile = await _mobileService.GetMobileByIdAsync(id, includeFeatures, includeBrand);
//            if (mobile == null)
//            {
//                return NotFound(new { Message = "Mobile not found" });
//            }

//            return Ok(new { Data = mobile, Message = "Mobile fetched successfully" });
//        }

//        [Authorize(Roles = "Admin, Employee")]
//        [HttpPatch("Update/{id}")]
//        public async Task<IActionResult> UpdateMobile(int id, [FromBody] UpdateMobileDto mobileDto)
//        {
//            if (mobileDto == null)
//            {
//                return BadRequest(new { Message = "Invalid update data" });
//            }

//            await _mobileService.UpdateMobileAsync(id, mobileDto);
//            return Ok(new { Message = "Mobile updated successfully" });
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpDelete("Delete/{id}")]
//        public async Task<IActionResult> DeleteMobile(int id)
//        {
//            await _mobileService.DeleteMobileAsync(id);
//            return Ok(new { Message = "Mobile deleted successfully" });
//        }
//    }
//}





























//using Microsoft.AspNetCore.Mvc;
//using WebApplication3.DTO;
//using WebApplication3.Services;
//using WebApplication3.Utils;
//using Microsoft.AspNetCore.Authorization;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using System.IO;
//using AutoMapper;
//using Microsoft.AspNetCore.OData.Query;
//using WebApplication3.Interfaces;

//namespace WebApplication3.Controllers
//{
//    [Route("api/Mobile")]
//    [ApiController]
//    public class MobileController : ControllerBase
//    {
//        private readonly IMobileService _mobileService;
//        private readonly CloudinaryService _cloudinaryService;
//        private readonly IMapper _mapper;

//        public MobileController(IMobileService mobileService, CloudinaryService cloudinaryService, IMapper mapper)
//        {
//            _mobileService = mobileService;
//            _cloudinaryService = cloudinaryService;
//            _mapper = mapper;
//        }

//        [Authorize(Roles = "Admin, Employee")]
//        [HttpPost("Create")]
//        public async Task<IActionResult> CreateMobile([FromForm] CreateMobileRequestDto mobileRequestDto)
//        {
//            if (mobileRequestDto == null)
//            {
//                return BadRequest(new { Message = "Invalid mobile data" });
//            }

//            var mobileDto = _mapper.Map<CreateMobileDto>(mobileRequestDto);

//            if (mobileRequestDto.ImageFile != null)
//            {
//                // Save file temporarily
//                var tempFilePath = Path.GetTempFileName();
//                using (var stream = new FileStream(tempFilePath, FileMode.Create))
//                {
//                    await mobileRequestDto.ImageFile.CopyToAsync(stream);
//                }

//                // Upload to Cloudinary
//                var imageUrl = await _cloudinaryService.UploadImageAsync(tempFilePath);
//                if (string.IsNullOrEmpty(imageUrl))
//                {
//                    return BadRequest(new { Message = "Image upload failed" });
//                }

//                // Set the Cloudinary image URL in DTO
//                mobileDto.MobileImage = imageUrl;
//            }

//            await _mobileService.AddMobileAsync(mobileDto);
//            return Ok(new { Message = "Mobile created successfully" });
//        }

//        //[AllowAnonymous] // Public Access
//        //[EnableQuery] // Enable OData Querying
//        //[HttpGet("Get-All")]
//        //public async Task<IActionResult> GetAllMobiles([FromQuery] bool includeFeatures = false)
//        //{
//        //    var mobiles = await _mobileService.GetAllMobilesAsync(includeFeatures);
//        //    return Ok(new { Data = mobiles, Message = "Fetched all mobiles successfully" });
//        //}


//        // Get => https://localhost:7045/api/Mobile/Get-All?$orderby=Price desc
//        // Get => https://localhost:7045/api/Mobile/Get-All?$select=Id,Name,Price
//        // Get => https://localhost:7045/api/Mobile/Get-All?$filter=Price lt 500
//        // Get => https://localhost:7045/api/Mobile/Get-All?$skip=0&$top=10
//        // Get => https://localhost:7045/api/Mobile/Get-All?$top=5

//        [AllowAnonymous]
//        [EnableQuery]
//        [HttpGet("Get-All")]
//        public IActionResult GetAllMobiles()
//        {
//            var mobiles = _mobileService.GetAllMobilesQueryable();
//            return Ok(mobiles);
//        }


//        [AllowAnonymous] // Public Access
//        [HttpGet("Get/{id}")]
//        public async Task<IActionResult> GetMobileById(int id, [FromQuery] bool includeFeatures = false)
//        {
//            var mobile = await _mobileService.GetMobileByIdAsync(id, includeFeatures);
//            if (mobile == null)
//            {
//                return NotFound(new { Message = "Mobile not found" });
//            }

//            return Ok(new { Data = mobile, Message = "Mobile fetched successfully" });
//        }

//        [Authorize(Roles = "Admin, Employee")]
//        [HttpPatch("Update/{id}")]
//        public async Task<IActionResult> UpdateMobile(int id, [FromForm] UpdateMobileDto mobileDto)
//        {
//            if (mobileDto == null)
//            {
//                return BadRequest(new { Message = "Invalid update data" });
//            }

//            if (mobileDto.ImageFile != null)
//            {
//                // Save file temporarily
//                var tempFilePath = Path.GetTempFileName();
//                using (var stream = new FileStream(tempFilePath, FileMode.Create))
//                {
//                    await mobileDto.ImageFile.CopyToAsync(stream);
//                }

//                // Upload to Cloudinary
//                var imageUrl = await _cloudinaryService.UploadImageAsync(tempFilePath);
//                if (string.IsNullOrEmpty(imageUrl))
//                {
//                    return BadRequest(new { Message = "Image upload failed" });
//                }

//                // Set the Cloudinary image URL in DTO
//                mobileDto.MobileImage = imageUrl;
//            }

//            await _mobileService.UpdateMobileAsync(id, mobileDto);
//            return Ok(new { Message = "Mobile updated successfully" });
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpDelete("Delete/{id}")]
//        public async Task<IActionResult> DeleteMobile(int id)
//        {
//            await _mobileService.DeleteMobileAsync(id);
//            return Ok(new { Message = "Mobile deleted successfully" });
//        }
//    }
//}




















using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTO;
using WebApplication3.Services;
using WebApplication3.Utils;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.OData.Query;
using WebApplication3.Interfaces;
using WebApplication3.Model;
using StackExchange.Redis;

namespace WebApplication3.Controllers
{
    [Route("api/Mobile")]
    [ApiController]
    public class MobileController : ControllerBase
    {
        private readonly IMobileService _mobileService;
        private readonly CloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IServiceProvider _serviceProvider;

        public MobileController(IMobileService mobileService, CloudinaryService cloudinaryService, IMapper mapper, ICacheService cacheService, IServiceProvider serviceProvider)
        {
            _mobileService = mobileService;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
            _cacheService = cacheService;
            _serviceProvider = serviceProvider;
        }

        [Authorize(Roles = "Admin, Employee")]
        [HttpPost("Create-Mobile")]
        public async Task<IActionResult> CreateMobile([FromForm] CreateMobileRequestDto mobileRequestDto)
        {
            try
            {
                if (mobileRequestDto == null)
                {
                    return BadRequest(new ApiError(400, "Invalid mobile data"));
                }

                var mobileDto = _mapper.Map<CreateMobileDto>(mobileRequestDto);

                if (mobileRequestDto.ImageFile != null)
                {
                    try
                    {
                        // Save file temporarily
                        var tempFilePath = Path.GetTempFileName();
                        using (var stream = new FileStream(tempFilePath, FileMode.Create))
                        {
                            await mobileRequestDto.ImageFile.CopyToAsync(stream);
                        }

                        // Upload to Cloudinary
                        var imageUrl = await _cloudinaryService.UploadImageAsync(tempFilePath);
                        if (string.IsNullOrEmpty(imageUrl))
                        {
                            return BadRequest(new ApiError(400, "Image upload failed"));
                        }

                        // Set the Cloudinary image URL in DTO
                        mobileDto.MobileImage = imageUrl;
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new ApiError(400, $"Image processing failed: {ex.Message}"));
                    }
                }

                var mobile = await _mobileService.AddMobileAsync(mobileDto);
                return Ok(new ApiResponse<MobileModel>(200, mobile, "Mobile created successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        // Get => https://localhost:7045/api/Mobile/Get-All-Mobiles?$orderby=Price desc
        // Get => https://localhost:7045/api/Mobile/Get-All-Mobiles?$select=Id,Name,Price
        // Get => https://localhost:7045/api/Mobile/Get-All-Mobiles?$filter=Price lt 500
        // Get => https://localhost:7045/api/Mobile/Get-All-Mobiles?$skip=0&$top=10
        // Get => https://localhost:7045/api/Mobile/Get-All-Mobiles?$top=5

        [AllowAnonymous]
        [EnableQuery]
        [HttpGet("Get-All-Mobiles")]
        public IActionResult GetAllMobiles()
        {
            try
            {
                var mobiles = _mobileService.GetAllMobilesQueryable();
                return Ok(mobiles);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        //[AllowAnonymous]
        //[HttpGet("Get-Mobiles-Paginated")]
        //public async Task<IActionResult> GetMobilesPaginated(int page = 1, int pageSize = 10, bool includeFeatures = false)
        //{
        //    try
        //    {
        //        if (page < 1 || pageSize < 1)
        //        {
        //            return BadRequest(new ApiError(400, "Invalid page or page size values"));
        //        }

        //        var (mobiles, totalMobiles) = await _mobileService.GetMobilesPaginatedAsync(page, pageSize, includeFeatures);

        //        return Ok(new ApiResponse<object>(
        //            200,
        //            new
        //            {
        //                Page = page,
        //                PageSize = pageSize,
        //                TotalMobiles = totalMobiles,
        //                TotalPages = (int)Math.Ceiling(totalMobiles / (double)pageSize),
        //                Mobiles = mobiles
        //            },
        //            "Pagination successful"
        //        ));
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(new ApiError(400, ex.Message));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new ApiError(500, $"An error occurred while retrieving paginated mobiles: {ex.Message}"));
        //    }
        //}


        [AllowAnonymous]
        [HttpGet("Get-Mobiles-Paginated")]
        public async Task<IActionResult> GetMobilesPaginated(int page = 1, int pageSize = 10, bool includeFeatures = false)
        {
            string cacheKey = $"Mobiles_Page{page}_Size{pageSize}_IncludeFeatures_{includeFeatures}";
            var cachedResult = await _cacheService.GetAsync<object>(cacheKey);

            if (cachedResult != null)
            {
                return Ok(new ApiResponse<object>(200, cachedResult, "Pagination successful (from cache)"));
            }

            var (mobiles, totalMobiles) = await _mobileService.GetMobilesPaginatedAsync(page, pageSize, includeFeatures);

            var response = new
            {
                Page = page,
                PageSize = pageSize,
                TotalMobiles = totalMobiles,
                TotalPages = (int)Math.Ceiling(totalMobiles / (double)pageSize),
                Mobiles = mobiles
            };

            await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5));
            return Ok(new ApiResponse<object>(200, response, "Pagination successful"));
        }


        //[AllowAnonymous] // Public Access
        //[HttpGet("Get-Mobile-By-Id/{id}")]
        //public async Task<IActionResult> GetMobileById(int id, [FromQuery] bool includeFeatures = false)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //        {
        //            return BadRequest(new ApiError(400, "Invalid mobile ID"));
        //        }

        //        var mobile = await _mobileService.GetMobileByIdAsync(id, includeFeatures);
        //        return Ok(new ApiResponse<MobileDetailsDto>(200, mobile, "Mobile fetched successfully"));
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(new ApiError(400, ex.Message));
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new ApiError(404, ex.Message));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new ApiError(500, $"An error occurred while retrieving mobile: {ex.Message}"));
        //    }
        //}

        //[AllowAnonymous] // Public Access
        [HttpGet("Get-Mobile-By-Id/{id}")]
        public async Task<IActionResult> GetMobileById(int id, [FromQuery] bool includeFeatures = false)
        {
            string cacheKey = $"MobileById_{id}_IncludeFeatures_{includeFeatures}";
            var cachedMobile = await _cacheService.GetAsync<MobileDetailsDto>(cacheKey);

            if (cachedMobile != null)
            {
                return Ok(new ApiResponse<MobileDetailsDto>(200, cachedMobile, "Mobile fetched from cache"));
            }

            var mobile = await _mobileService.GetMobileByIdAsync(id, includeFeatures);
            await _cacheService.SetAsync(cacheKey, mobile, TimeSpan.FromMinutes(5));

            return Ok(new ApiResponse<MobileDetailsDto>(200, mobile, "Mobile fetched successfully"));
        }


        //[Authorize(Roles = "Admin, Employee")]
        //[HttpPatch("Update-Mobile/{id}")]
        //public async Task<IActionResult> UpdateMobile(int id, [FromBody] UpdateMobileDto mobileDto)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //        {
        //            return BadRequest(new ApiError(400, "Invalid mobile ID"));
        //        }

        //        if (mobileDto == null)
        //        {
        //            return BadRequest(new ApiError(400, "Invalid update data"));
        //        }

        //        if (mobileDto.ImageFile != null)
        //        {
        //            try
        //            {
        //                // Save file temporarily
        //                var tempFilePath = Path.GetTempFileName();
        //                using (var stream = new FileStream(tempFilePath, FileMode.Create))
        //                {
        //                    await mobileDto.ImageFile.CopyToAsync(stream);
        //                }

        //                // Upload to Cloudinary
        //                var imageUrl = await _cloudinaryService.UploadImageAsync(tempFilePath);
        //                if (string.IsNullOrEmpty(imageUrl))
        //                {
        //                    return BadRequest(new ApiError(400, "Image upload failed"));
        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                return BadRequest(new ApiError(400, $"Image processing failed: {ex.Message}"));
        //            }
        //        }

        //        await _mobileService.UpdateMobileAsync(id, mobileDto);
        //        return Ok(new ApiResponse<string>(200, "Mobile updated successfully"));
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new ApiError(404, ex.Message));
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(new ApiError(400, ex.Message));
        //    }
        //}







        //[Authorize(Roles = "Admin, Employee")]
        //[HttpPatch("Update-Mobile/{id}")]
        //public async Task<IActionResult> UpdateMobile(int id, [FromBody] UpdateMobileDto mobileDto)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //        {
        //            return BadRequest(new ApiError(400, "Invalid mobile ID"));
        //        }
        //        if (mobileDto == null)
        //        {
        //            return BadRequest(new ApiError(400, "Invalid update data"));
        //        }

        //        // Pass the imageUrl to the service method
        //        await _mobileService.UpdateMobileAsync(id, mobileDto);
        //        return Ok(new ApiResponse<string>(200, "Mobile updated successfully"));
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new ApiError(404, ex.Message));
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(new ApiError(400, ex.Message));
        //    }
        //}


        [Authorize(Roles = "Admin, Employee")]
        [HttpPatch("Update-Mobile/{id}")]
        public async Task<IActionResult> UpdateMobile(int id, [FromBody] UpdateMobileDto mobileDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiError(400, "Invalid mobile ID"));
                }
                if (mobileDto == null)
                {
                    return BadRequest(new ApiError(400, "Invalid update data"));
                }

                // Pass the imageUrl to the service method
                await _mobileService.UpdateMobileAsync(id, mobileDto);

                // Clear all related cache entries
                await InvalidateMobileCacheAsync(id);

                return Ok(new ApiResponse<string>(200, "Mobile updated successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiError(404, ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        //[Authorize(Roles = "Admin")]
        //[HttpDelete("Delete-Mobile/{id}")]
        //public async Task<IActionResult> DeleteMobile(int id)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //        {
        //            return BadRequest(new ApiError(400, "Invalid mobile ID"));
        //        }

        //        await _mobileService.DeleteMobileAsync(id);
        //        return Ok(new ApiResponse<string>(200, "Mobile deleted successfully"));
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(new ApiError(400, ex.Message));
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new ApiError(404, ex.Message));
        //    }
        //}



        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete-Mobile/{id}")]
        public async Task<IActionResult> DeleteMobile(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiError(400, "Invalid mobile ID"));
                }

                await _mobileService.DeleteMobileAsync(id);

                // Clear all related cache entries
                await InvalidateMobileCacheAsync(id);

                return Ok(new ApiResponse<string>(200, "Mobile deleted successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiError(404, ex.Message));
            }
        }

        // Helper method to invalidate all mobile-related cache entries
        private async Task InvalidateMobileCacheAsync(int mobileId)
        {
            // First, clear the memory cache for specific mobile
            _cacheService.Remove($"MobileById_{mobileId}_IncludeFeatures_True");
            _cacheService.Remove($"MobileById_{mobileId}_IncludeFeatures_False");

            // Now use the Redis connection to efficiently clear all paginated caches
            // This requires adding IConnectionMultiplexer to your controller dependencies
            var connectionMultiplexer = _serviceProvider.GetRequiredService<IConnectionMultiplexer>();

            // Clear specific mobile caches
            await RemoveKeysByPatternAsync(connectionMultiplexer, $"MobileById_{mobileId}_*");

            // Clear all paginated caches that might contain this mobile
            await RemoveKeysByPatternAsync(connectionMultiplexer, "Mobiles_Page*");
        }

        private async Task RemoveKeysByPatternAsync(IConnectionMultiplexer connectionMultiplexer, string pattern)
        {
            var endpoints = connectionMultiplexer.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = connectionMultiplexer.GetServer(endpoint);
                var keys = server.Keys(pattern: pattern).ToArray();

                if (keys.Length > 0)
                {
                    var db = connectionMultiplexer.GetDatabase();
                    foreach (var key in keys)
                    {
                        // Remove from Redis
                        await db.KeyDeleteAsync(key);

                        // Also remove from memory cache
                        _cacheService.Remove(key.ToString());
                    }
                }
            }
        }

    }
}