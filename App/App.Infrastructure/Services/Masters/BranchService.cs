using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Application.Interfaces.Services.Masters;
using App.Domain.Entities;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Extensions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Infrastructure.Services.Masters
{
    public class BranchService : IBranchService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblBranch> _BranchRepo;
        private readonly IGenericRepository<VwBranchDetail> _vwBranchDetail;
        private readonly ILogger<BranchService> _logger;
        private readonly IContextService _userService;

        public BranchService(IGenericRepository<TblBranch> BranchRepo,
            IGenericRepository<VwBranchDetail> vwBranchDetail
            , IMapper mapper
            , ILogger<BranchService> logger
            , IContextService userService)
        {
            _BranchRepo = BranchRepo;
            _vwBranchDetail = vwBranchDetail; // assign
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
        }

        public async Task<int> DeleteAsync(string code)
        {
            try
            {
                var entityItem = await _BranchRepo.FindAsync(t => t.BranchCode == code)
                      ?? throw new KeyNotFoundException($"Code {code} not found");
                return await _BranchRepo.Remove(entityItem);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, $"Delete blocked by FK for Code : {code}");
                throw new Exception(
                    $"Oops, this record ({code}) is used by another configuration and cannot be deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Branch with code {code}", code);
                throw;
            }

        }

        public async Task<BranchDto> GetByCodeAsync(string code)
        {
            try
            {
                var entityItem = await _vwBranchDetail.FindAsync(t => t.BranchCode == code);
                return _mapper.Map<BranchDto>(entityItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Branch with code {code}", code);
                throw;
            }

        }

        public async Task<PagedResponse<BranchDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _BranchRepo.GetPagedAsync(model);
                return entityResult.MapPaged<TblBranch, BranchDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paged Branch data");
                throw;
            }
        }

        public async Task<List<BranchDto>> GetListAsync()
        {
            try
            {
                var entityResult = await _BranchRepo.GetListAsync();
                return _mapper.Map<List<BranchDto>>(entityResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Branch list");
                throw;
            }
        }

        public async Task<BranchDto> SaveAsync(BranchDto model)
        {
            try
            {
                var entityItem = await _BranchRepo.FindAsync(t => t.BranchCode.Equals(model.BranchCode));
                if (entityItem == null)
                {
                    TblBranch item = _mapper.Map<TblBranch>(model);
                    item.CreatedDate = DateTime.Now;
                    item.CreatedBy = _userService.Username;
                    var addedEntity = await _BranchRepo.AddAsync(item);
                    return _mapper.Map<BranchDto>(addedEntity);
                }
                else
                {
                    _mapper.Map(model, entityItem);
                    entityItem.UpdatedDate = DateTime.Now;
                    entityItem.UpdatedBy = _userService.Username;
                    var updatedEntity = await _BranchRepo.UpdateAsync(entityItem);
                    return _mapper.Map<BranchDto>(updatedEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Branch");
                throw;
            }
        }
        public Task<int> Delete(string code)
        {
            throw new NotImplementedException();
        }
    }
}
