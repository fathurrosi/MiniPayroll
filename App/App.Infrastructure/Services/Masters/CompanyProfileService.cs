using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services.Masters;
using App.Domain.Entities;
using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response; 
using MapsterMapper;
using Microsoft.Extensions.Logging; 

namespace App.Infrastructure.Services.Masters
{
    public class CompanyProfileService : IProfileService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblCompanyProfile> _companyProfileRepo;
        private readonly ILogger<CompanyProfileService> _logger;
        public CompanyProfileService(
            ILogger<CompanyProfileService> logger,
           IGenericRepository<TblCompanyProfile> companyProfileRepo,
            IMapper mapper)
        {
            _logger = logger;
            _companyProfileRepo = companyProfileRepo;
            _mapper = mapper;
        }

        public async Task<List<ProfileDto>> GetAllAsync()
        {
            var entities = await _companyProfileRepo.GetListAsync();

            return _mapper.Map<List<ProfileDto>>(entities);
        }

        public async Task<ProfileDto?> GetByIdAsync(int companyProfileId)
        {
            var entity = await _companyProfileRepo.GetFirstOrDefaultAsync(x =>
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

        public async Task<bool> DeleteAsync(int companyProfileId)
        {
            var entity = await _companyProfileRepo.FindAsync(x =>
                    x.CompanyProfileId == companyProfileId);

            if (entity == null)
                return false;

            await _companyProfileRepo.Remove(entity);


            return true;
        }

        public Task<PagedResponse<ProfileDto>> GetPagedAsync(DataTableRequest model)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProfileDto>> GetCountriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProfileDto> Save(ProfileDto model)
        {
            throw new NotImplementedException();
        }

        public Task<int> Delete(string code)
        {
            throw new NotImplementedException();
        }

        public Task<ProfileDto> GetByCode(string code)
        {
            throw new NotImplementedException();
        }
    }
}
