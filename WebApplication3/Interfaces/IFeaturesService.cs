using WebApplication3.DTO;

namespace WebApplication3.Interfaces
{
    public interface IFeaturesService
    {
        public Task AddFeaturesAsync(FeaturesCreateDTO featuresDto);

        public Task<FeaturesResponseDTO?> GetFeaturesByMobileIdAsync(int mobileId);

        public Task UpdateFeaturesAsync(int mobileId, FeaturesCreateDTO featuresDto);

        public Task DeleteFeaturesAsync(int mobileId);
    }
}
