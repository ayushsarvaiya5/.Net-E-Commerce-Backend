using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Interfaces;
using WebApplication3.Model;

namespace WebApplication3.Repositories
{
    public class FeaturesRepository : IFeaturesRepository
    {
        private readonly MobileDbContext _db;

        public FeaturesRepository(MobileDbContext db)
        {
            _db = db;
        }

        public async Task AddFeaturesAsync(FeaturesModel features)
        {
            await _db.Features.AddAsync(features);
            await _db.SaveChangesAsync();
        }

        public async Task<FeaturesModel?> GetFeaturesByMobileIdAsync(int mobileId)
        {
            return await _db.Features.FirstOrDefaultAsync(f => f.MobileId == mobileId);
        }

        public async Task UpdateFeaturesAsync(FeaturesModel features)
        {
            _db.Features.Update(features);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteFeaturesAsync(FeaturesModel features)
        {
            _db.Features.Remove(features);
            await _db.SaveChangesAsync();
        }
    }
}
