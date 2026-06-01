using App.Application.Interfaces.Repositories;
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
        public CompanyProfileService(
            ILogger<CompanyProfileService> logger,
           IGenericRepository<TblCompanyProfile> profileRepo,
            IMapper mapper)
        {
            _logger = logger;
            _profileRepo = profileRepo;
            _mapper = mapper;
        }

        public async Task<List<ProfileDto>> GetAllAsync()
        {
            var entities = await _profileRepo.GetListAsync();

            return _mapper.Map<List<ProfileDto>>(entities);
        }

        public async Task<ProfileDto?> GetByIdAsync(int companyProfileId)
        {
            var entity = await _profileRepo.GetFirstOrDefaultAsync(x =>
                    x.CompanyProfileId == companyProfileId);

            return entity == null
                ? null
                : _mapper.Map<ProfileDto>(entity);
        }

        public async Task<int> CreateAsync(ProfileDto dto)
        {
            //var entity = _mapper.Map<CompanyProfile>(dto);

            //entity.CreatedDate = DateTime.Now;

            //_context.CompanyProfiles.Add(entity);

            //await _context.SaveChangesAsync();

            return 1;
        }

        public async Task<bool> UpdateAsync(ProfileDto dto)
        {
            //var entity = await _context.CompanyProfiles
            //    .FirstOrDefaultAsync(x =>
            //        x.CompanyProfileId == dto.CompanyProfileId);

            //if (entity == null)
            //    return false;

            //entity.CompanyName = dto.CompanyName;
            //entity.CompanyAddress = dto.CompanyAddress;
            //entity.PhoneNumber = dto.PhoneNumber;
            //entity.Email = dto.Email;
            //entity.Website = dto.Website;
            //entity.TaxNumber = dto.TaxNumber;
            //entity.LogoFileName = dto.LogoFileName;
            //entity.LogoFilePath = dto.LogoFilePath;
            //entity.IsActive = dto.IsActive;
            //entity.UpdatedDate = DateTime.Now;

            //await _context.SaveChangesAsync();

            return true;
        }

        public async Task<int> DeleteAsync(int companyProfileId)
        {
            var entity = await _profileRepo.FindAsync(x =>
                x.CompanyProfileId == companyProfileId);

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


         
        public async Task<ProfileDto> Save(ProfileDto model)
        {
            try
            {
                var entityItem = await _profileRepo.FindAsync(t => t.CompanyProfileId.Equals(model.CompanyProfileId));
                if (entityItem == null)
                {
                    TblCompanyProfile item = _mapper.Map<TblCompanyProfile>(model);
                    var addedEntity = await _profileRepo.AddAsync(item);
                    return _mapper.Map<ProfileDto>(addedEntity);
                }
                else
                {
                    _mapper.Map(model, entityItem);
                    var updatedEntity = await _profileRepo.UpdateAsync(entityItem);
                    return _mapper.Map<ProfileDto>(updatedEntity);
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
