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
    public class LeaveTypeService : ILeaveTypeService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblLeaveType> _LeaveTypeRepo;
        private readonly ILogger<LeaveTypeService> _logger;
        private readonly IContextService _userService;

        public LeaveTypeService(IGenericRepository<TblLeaveType> LeaveTypeRepo
            , IMapper mapper
            , ILogger<LeaveTypeService> logger
            , IContextService userService)
        {
            _LeaveTypeRepo = LeaveTypeRepo;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
        }

        public async Task<int> DeleteAsync(string code)
        {
            try
            {
                var entityItem = await _LeaveTypeRepo.FindAsync(t => t.LeaveCode == code)
                      ?? throw new KeyNotFoundException($"Code {code} not found");
                return await _LeaveTypeRepo.Remove(entityItem);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, $"Delete blocked by FK for Code : {code}");
                throw new Exception(
                    $"Oops, this record ({code}) is used by another configuration and cannot be deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting LeaveType with code {code}", code);
                throw;
            }

        }

        public async Task<LeaveTypeDto> GetByCodeAsync(string code)
        {
            try
            {
                var entityItem = await _LeaveTypeRepo.FindAsync(t => t.LeaveCode == code);
                return _mapper.Map<LeaveTypeDto>(entityItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving LeaveType with code {code}", code);
                throw;
            }

        }

        public async Task<PagedResponse<LeaveTypeDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _LeaveTypeRepo.GetPagedAsync(model);
                return entityResult.MapPaged<TblLeaveType, LeaveTypeDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paged LeaveType data");
                throw;
            }
        }

        public async Task<List<LeaveTypeDto>> GetListAsync()
        {
            try
            {
                var entityResult = await _LeaveTypeRepo.GetListAsync();
                return _mapper.Map<List<LeaveTypeDto>>(entityResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving LeaveType list");
                throw;
            }
        }

        public async Task<LeaveTypeDto> SaveAsync(LeaveTypeDto model)
        {
            try
            {
                var entityItem = await _LeaveTypeRepo.FindAsync(t => t.LeaveCode.Equals(model.LeaveCode));
                if (entityItem == null)
                {
                    TblLeaveType item = _mapper.Map<TblLeaveType>(model);
                    item.CreatedDate = DateTime.Now;
                    item.CreatedBy = _userService.Username;
                    var addedEntity = await _LeaveTypeRepo.AddAsync(item);
                    return _mapper.Map<LeaveTypeDto>(addedEntity);
                }
                else
                {
                    _mapper.Map(model, entityItem);
                    entityItem.UpdatedDate = DateTime.Now;
                    entityItem.UpdatedBy = _userService.Username;
                    var updatedEntity = await _LeaveTypeRepo.UpdateAsync(entityItem);
                    return _mapper.Map<LeaveTypeDto>(updatedEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving LeaveType");
                throw;
            }
        }
        public Task<int> Delete(string code)
        {
            throw new NotImplementedException();
        }
    }
}
