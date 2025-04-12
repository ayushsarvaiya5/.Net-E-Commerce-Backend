using WebApplication3.Model;

namespace WebApplication3.Interfaces
{
    public interface IMobileRepository
    {
        Task<IEnumerable<MobileModel>> GetAllAsync(bool includeBrand = false);
        IQueryable<MobileModel> GetAllQueryable();
        Task<MobileModel?> GetByIdAsync(int id, bool includeFeatures = false);
        Task AddAsync(MobileModel mobile);
        Task UpdateAsync(MobileModel mobile);
        Task DeleteAsync(int id);
        Task<(List<MobileModel>, int)> GetMobilesPaginatedAsync(int page, int pageSize, bool includeFeatures = false);
    }
}
