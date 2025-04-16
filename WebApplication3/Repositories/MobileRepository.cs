
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Interfaces;
using WebApplication3.Model;

namespace WebApplication3.Repositories
{
    public class MobileRepository : IMobileRepository
    {
        private readonly MobileDbContext _context;

        public MobileRepository(MobileDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MobileModel>> GetAllAsync(bool includeFeatures = false)
        {
            try
            {
                IQueryable<MobileModel> query = _context.Mobiles.Include(m => m.Brand);

                if (includeFeatures)
                    query = query.Include(m => m.Features);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving all mobiles: {ex.Message}", ex);
            }
        }

        public IQueryable<MobileModel> GetAllQueryable()
        {
            try
            {
                IQueryable<MobileModel> query = _context.Mobiles;
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving mobile queryable: {ex.Message}", ex);
            }
        }

        public async Task<(List<MobileModel>, int)> GetMobilesPaginatedAsync(int page, int pageSize, bool includeFeatures = false)
        {
            try
            {
                if (page < 1 || pageSize < 1)
                    throw new ArgumentException("Invalid page or page size values");

                var totalMobiles = await _context.Mobiles.CountAsync();

                IQueryable<MobileModel> query = _context.Mobiles.Include(m => m.Brand);

                if (includeFeatures)
                    query = query.Include(m => m.Features);

                var mobiles = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (mobiles, totalMobiles);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving paginated mobiles: {ex.Message}", ex);
            }
        }

        public async Task<MobileModel?> GetByIdAsync(int id, bool includeFeatures = false)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid mobile ID");

                IQueryable<MobileModel> query = _context.Mobiles.Include(m => m.Brand);

                if (includeFeatures)
                    query = query.Include(m => m.Features);

                return await query.FirstOrDefaultAsync(m => m.Id == id);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving mobile with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task AddAsync(MobileModel mobile)
        {
            try
            {
                if (mobile == null)
                    throw new ArgumentNullException(nameof(mobile), "Mobile cannot be null");

                await _context.Mobiles.AddAsync(mobile);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error saving mobile to database: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding mobile: {ex.Message}", ex);
            }
        }

        public async Task UpdateAsync(MobileModel mobile)
        {
            try
            {
                if (mobile == null)
                    throw new ArgumentNullException(nameof(mobile), "Mobile cannot be null");

                _context.Mobiles.Update(mobile);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error updating mobile in database: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating mobile: {ex.Message}", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid mobile ID");

                var mobile = await _context.Mobiles.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.Id == id);
                if (mobile == null)
                    throw new KeyNotFoundException($"Mobile with ID {id} not found");

                mobile.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error soft deleting mobile: {ex.Message}", ex);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error soft deleting mobile with ID {id}: {ex.Message}", ex);
            }
        }

    }
}