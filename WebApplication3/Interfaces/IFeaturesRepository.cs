using WebApplication3.Model;

namespace WebApplication3.Interfaces
{
    public interface IFeaturesRepository
    {
        Task AddFeaturesAsync(FeaturesModel features);
        Task<FeaturesModel?> GetFeaturesByMobileIdAsync(int mobileId);
        Task UpdateFeaturesAsync(FeaturesModel features);
        Task DeleteFeaturesAsync(FeaturesModel features);
    }
}
