//using AutoMapper;
//using WebApplication3.DTO;
//using WebApplication3.Interfaces;
//using WebApplication3.Model;

//namespace WebApplication3.Services
//{
//    public class MobileService : IMobileService
//    {
//        private readonly IMobileRepository _mobileRepository;
//        private readonly IMapper _mapper;

//        public MobileService(IMobileRepository mobileRepository, IMapper mapper)
//        {
//            _mobileRepository = mobileRepository;
//            _mapper = mapper;
//        }

//        //public async Task<IEnumerable<MobileDetailsDto>> GetAllMobilesAsync(bool includeFeatures = false)
//        //{
//        //    var mobiles = await _mobileRepository.GetAllAsync(includeFeatures);
//        //    return _mapper.Map<IEnumerable<MobileDetailsDto>>(mobiles);
//        //}

//        public IQueryable<MobileDetailsDto> GetAllMobilesQueryable()
//        {
//            var mobilesQuery = _mobileRepository.GetAllQueryable();
//            return _mapper.ProjectTo<MobileDetailsDto>(mobilesQuery);
//        }


//        public async Task<MobileDetailsDto?> GetMobileByIdAsync(int id, bool includeFeatures = false)
//        {
//            var mobile = await _mobileRepository.GetByIdAsync(id, includeFeatures);
//            return mobile != null ? _mapper.Map<MobileDetailsDto>(mobile) : null;
//        }

//        public async Task AddMobileAsync(CreateMobileDto mobileDto)
//        {
//            var mobile = _mapper.Map<MobileModel>(mobileDto);
//            await _mobileRepository.AddAsync(mobile);
//        }

//        public async Task UpdateMobileAsync(int id, UpdateMobileDto mobileDto)
//        {
//            var mobile = await _mobileRepository.GetByIdAsync(id);
//            if (mobile != null)
//            {
//                _mapper.Map(mobileDto, mobile);
//                await _mobileRepository.UpdateAsync(mobile);
//            }
//        }

//        public async Task DeleteMobileAsync(int id)
//        {
//            await _mobileRepository.DeleteAsync(id);
//        }
//    }
//}














using AutoMapper;
using WebApplication3.DTO;
using WebApplication3.Interfaces;
using WebApplication3.Model;

namespace WebApplication3.Services
{
    public class MobileService : IMobileService
    {
        private readonly IMobileRepository _mobileRepository;
        private readonly IMapper _mapper;

        public MobileService(IMobileRepository mobileRepository, IMapper mapper)
        {
            _mobileRepository = mobileRepository;
            _mapper = mapper;
        }

        public IQueryable<MobileDetailsDto> GetAllMobilesQueryable()
        {
            try
            {
                var mobilesQuery = _mobileRepository.GetAllQueryable();
                return _mapper.ProjectTo<MobileDetailsDto>(mobilesQuery);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving mobile queryable: {ex.Message}", ex);
            }
        }

        public async Task<(List<MobileDetailsDto>, int)> GetMobilesPaginatedAsync(int page, int pageSize, bool includeFeatures = false)
        {
            try
            {
                if (page < 1 || pageSize < 1)
                    throw new ArgumentException("Invalid page or page size values");

                var (mobiles, totalMobiles) = await _mobileRepository.GetMobilesPaginatedAsync(page, pageSize, includeFeatures);
                var mobilesDto = _mapper.Map<List<MobileDetailsDto>>(mobiles);
                return (mobilesDto, totalMobiles);
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

        public async Task<MobileDetailsDto?> GetMobileByIdAsync(int id, bool includeFeatures = false)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid mobile ID");

                var mobile = await _mobileRepository.GetByIdAsync(id, includeFeatures);
                if (mobile == null)
                    throw new KeyNotFoundException($"Mobile with ID {id} not found");

                return _mapper.Map<MobileDetailsDto>(mobile);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving mobile with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<MobileModel> AddMobileAsync(CreateMobileDto mobileDto)
        {
            try
            {
                if (mobileDto == null)
                    throw new ArgumentNullException(nameof(mobileDto), "Mobile data cannot be null");

                var mobile = _mapper.Map<MobileModel>(mobileDto);
                await _mobileRepository.AddAsync(mobile);

                return mobile;
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding mobile: {ex.Message}", ex);
            }
        }

        //public async Task UpdateMobileAsync(int id, UpdateMobileDto mobileDto)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //            throw new ArgumentException("Invalid mobile ID");

        //        if (mobileDto == null)
        //            throw new ArgumentNullException(nameof(mobileDto), "Mobile update data cannot be null");

        //        var mobile = await _mobileRepository.GetByIdAsync(id);
        //        if (mobile == null)
        //            throw new KeyNotFoundException($"Mobile with ID {id} not found");

        //        _mapper.Map(mobileDto, mobile);
        //        await _mobileRepository.UpdateAsync(mobile);
        //    }

        //    catch (KeyNotFoundException)
        //    {
        //        throw;
        //    }
        //    catch (ArgumentException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error updating mobile with ID {id}: {ex.Message}", ex);
        //    }
        //}


        public async Task UpdateMobileAsync(int id, UpdateMobileDto mobileDto)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid mobile ID");

                if (mobileDto == null)
                    throw new ArgumentNullException(nameof(mobileDto), "Mobile update data cannot be null");

                var mobile = await _mobileRepository.GetByIdAsync(id);
                if (mobile == null)
                    throw new KeyNotFoundException($"Mobile with ID {id} not found");

                // Manual mapping instead of using AutoMapper to preserve existing values
                if (mobileDto.Name != null)
                    mobile.Name = mobileDto.Name;

                if (mobileDto.Description != null)
                    mobile.Description = mobileDto.Description;

                if (mobileDto.Price.HasValue)
                    mobile.Price = mobileDto.Price.Value;

                if (mobileDto.StockQuantity.HasValue)
                    mobile.StockQuantity = mobileDto.StockQuantity.Value;

                if (mobileDto.BrandId.HasValue)
                    mobile.BrandId = mobileDto.BrandId.Value;

                await _mobileRepository.UpdateAsync(mobile);
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
                throw new Exception($"Error updating mobile with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task DeleteMobileAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid mobile ID");

                await _mobileRepository.DeleteAsync(id);
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
                throw new Exception($"Error deleting mobile with ID {id}: {ex.Message}", ex);
            }
        }
    }
}   