using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTO;
using WebApplication3.Interfaces;
using WebApplication3.Services;

namespace WebApplication3.Controllers
{
    [Route("api/Features")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
        private readonly IFeaturesService _featuresService;

        public FeaturesController(IFeaturesService featuresService)
        {
            _featuresService = featuresService;
        }

        [Authorize(Roles = "Admin, Employee")]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateFeatures([FromBody] FeaturesCreateDTO featuresDto)
        {
            if (featuresDto == null)
            {
                return BadRequest(new { Message = "Invalid features data" });
            }

            await _featuresService.AddFeaturesAsync(featuresDto);
            return Ok(new { Message = "Features added successfully" });
        }

        [AllowAnonymous]
        [HttpGet("Get/{mobileId}")]
        public async Task<IActionResult> GetFeatures(int mobileId)
        {
            var features = await _featuresService.GetFeaturesByMobileIdAsync(mobileId);
            if (features == null)
            {
                return NotFound(new { Message = "Features not found" });
            }

            return Ok(new { Data = features, Message = "Features fetched successfully" });
        }

        [Authorize(Roles = "Admin, Employee")]
        [HttpPatch("Update/{mobileId}")]
        public async Task<IActionResult> UpdateFeatures(int mobileId, [FromBody] FeaturesCreateDTO featuresDto)
        {
            if (featuresDto == null)
            {
                return BadRequest(new { Message = "Invalid update data" });
            }

            await _featuresService.UpdateFeaturesAsync(mobileId, featuresDto);
            return Ok(new { Message = "Features updated successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{mobileId}")]
        public async Task<IActionResult> DeleteFeatures(int mobileId)
        {
            await _featuresService.DeleteFeaturesAsync(mobileId);
            return Ok(new { Message = "Features deleted successfully" });
        }
    }
}
