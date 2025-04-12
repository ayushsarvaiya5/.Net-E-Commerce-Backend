using AutoMapper;
using WebApplication3.DTO;
using WebApplication3.Interfaces;
using WebApplication3.Model;

namespace WebApplication3.Services
{
    public class FeaturesService : IFeaturesService
    {
        private readonly IFeaturesRepository _featuresRepository;
        private readonly IMapper _mapper;

        public FeaturesService(IFeaturesRepository featuresRepository, IMapper mapper)
        {
            _featuresRepository = featuresRepository;
            _mapper = mapper;
        }

        public async Task AddFeaturesAsync(FeaturesCreateDTO featuresDto)
        {
            var features = _mapper.Map<FeaturesModel>(featuresDto);
            await _featuresRepository.AddFeaturesAsync(features);
        }

        public async Task<FeaturesResponseDTO?> GetFeaturesByMobileIdAsync(int mobileId)
        {
            var features = await _featuresRepository.GetFeaturesByMobileIdAsync(mobileId);
            return _mapper.Map<FeaturesResponseDTO>(features);
        }

        public async Task UpdateFeaturesAsync(int mobileId, FeaturesCreateDTO featuresDto)
        {
            var features = await _featuresRepository.GetFeaturesByMobileIdAsync(mobileId);
            if (features == null) throw new KeyNotFoundException("Features not found.");

            _mapper.Map(featuresDto, features);
            await _featuresRepository.UpdateFeaturesAsync(features);
        }

        public async Task DeleteFeaturesAsync(int mobileId)
        {
            var features = await _featuresRepository.GetFeaturesByMobileIdAsync(mobileId);
            if (features == null) throw new KeyNotFoundException("Features not found.");

            await _featuresRepository.DeleteFeaturesAsync(features);
        }
    }
}
