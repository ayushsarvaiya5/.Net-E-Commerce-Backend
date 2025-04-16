
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

        [AllowAnonymous]
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