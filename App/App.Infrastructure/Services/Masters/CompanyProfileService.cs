using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Application.Interfaces.Services.Masters;
using App.Domain.Entities;
using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Extensions;
using MapsterMapper; 
using Microsoft.Extensions.Logging;

namespace App.Infrastructure.Services.Masters
{
    public class CompanyProfileService : IProfileService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblCompanyProfile> _profileRepo;
        private readonly ILogger<CompanyProfileService> _logger;
        private readonly IContextService _context;
        public CompanyProfileService(
            ILogger<CompanyProfileService> logger,
           IGenericRepository<TblCompanyProfile> profileRepo,
           IContextService context,
            IMapper mapper)
        {
            _logger = logger;
            _profileRepo = profileRepo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<ProfileDto>> GetListAsync()
        {
            var entities = await _profileRepo.GetListAsync();
            return _mapper.Map<List<ProfileDto>>(entities);
        }

        public async Task<ProfileDto?> GetByIdAsync(int id)
        {
            var entity = await _profileRepo.GetFirstOrDefaultAsync(x =>
                    x.CompanyProfileId == id);

            return entity == null
                ? null
                : _mapper.Map<ProfileDto>(entity);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _profileRepo.FindAsync(x =>
                x.CompanyProfileId == id);
            if (entity == null)
                return 0;

            return await _profileRepo.Remove(entity);
        }

        public async Task<PagedResponse<ProfileDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _profileRepo.GetPagedAsync(model);
                return entityResult.MapPaged<TblCompanyProfile, ProfileDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged Employees");
                throw;
            }
        }



        public async Task<ProfileDto> SaveAsync(ProfileDto model)
        {
            try
            {
                var existingItem = await _profileRepo.FindAsync(t => t.CompanyProfileId.Equals(model.CompanyProfileId));
                if (existingItem == null)
                {
                    var item = _mapper.Map<TblCompanyProfile>(model);
                    item.CreatedBy = _context.Username;
                    item.CreatedDate = DateTime.Now;
                    var addedItem = await _profileRepo.AddAsync(item);
                    return _mapper.Map<ProfileDto>(addedItem);
                }
                else
                {
                    _mapper.Map(model, existingItem);
                    existingItem.UpdatedBy = _context.Username;
                    existingItem.UpdatedDate = DateTime.Now;
                    var updatedItem = await _profileRepo.UpdateAsync(existingItem);
                    return _mapper.Map<ProfileDto>(updatedItem);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving profile");
                throw;
            }
        }
    }
}
