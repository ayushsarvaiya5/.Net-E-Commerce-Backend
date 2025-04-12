using WebApplication3.DTO;
using WebApplication3.Model;

namespace WebApplication3.Interfaces
{
    public interface IMobileService
    {
        public IQueryable<MobileDetailsDto> GetAllMobilesQueryable();

        public Task<MobileDetailsDto?> GetMobileByIdAsync(int id, bool includeFeatures = false);

        public Task<MobileModel> AddMobileAsync(CreateMobileDto mobileDto);

        public Task UpdateMobileAsync(int id, UpdateMobileDto mobileDto);

        public Task DeleteMobileAsync(int id);
        Task<(List<MobileDetailsDto>, int)> GetMobilesPaginatedAsync(int page, int pageSize, bool includeFeatures = false);
    }
}
