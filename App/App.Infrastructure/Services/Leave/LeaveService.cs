using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Application.Interfaces.Services.Leave;
using App.Application.Interfaces.Services.Masters;
using App.Domain.Entities;
using App.Domain.Models.Dto.Leave;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Extensions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Infrastructure.Services.Leave
{
    public class LeaveService : ILeaveService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblLeaveRequest> _leaveRepo;
        private readonly ILogger<LeaveService> _logger;
        private readonly IContextService _userService;

        public LeaveService(IGenericRepository<TblLeaveRequest> leaveRepo
            , IMapper mapper
            , ILogger<LeaveService> logger
            , IContextService userService)
        {
            _leaveRepo = leaveRepo;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
        }

        public async Task<int> DeleteAsync(long id)
        {
            try
            {
                var entityItem = await _leaveRepo.FindAsync(t => t.Id == id)
                      ?? throw new KeyNotFoundException($"Leave with ID {id} not found");
                return await _leaveRepo.Remove(entityItem);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, $"Delete blocked by FK for Leave ID : {id}");
                throw new Exception(
                    $"Oops, this record (ID: {id}) is used by another configuration and cannot be deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Leave with ID {id}", id);
                throw;
            }
        }

        public async Task<LeaveDto> GetByCodeAsync(int Id)
        {
            try
            {
                var entityItem = await _leaveRepo.FindAsync(t => t.Id == Id);
                return _mapper.Map<LeaveDto>(entityItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Leave with Id {Id}", Id);
                throw;
            }
        }

        public async Task<PagedResponse<LeaveDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _leaveRepo.GetPagedAsync(model);
                return entityResult.MapPaged<TblLeaveRequest, LeaveDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paged Leave data");
                throw;
            }
        }

        public async Task<List<LeaveDto>> GetListAsync()
        {
            try
            {
                var entityList = await _leaveRepo.GetListAsync();
                return _mapper.Map<List<LeaveDto>>(entityList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Leave list");
                throw;
            }
        }

        public async Task<LeaveDto> SaveAsync(LeaveDto model)
        {
            try
            {
                var entityItem = await _leaveRepo.FindAsync(t => t.Id.Equals(model.Id));
                if (entityItem == null)
                {
                    TblLeaveRequest item = _mapper.Map<TblLeaveRequest>(model);
                    item.CreatedDate = DateTime.Now;
                    item.CreatedBy = _userService.Username;
                    var addedEntity = await _leaveRepo.AddAsync(item);
                    return _mapper.Map<LeaveDto>(addedEntity);
                }
                else
                {
                    _mapper.Map(model, entityItem);
                    entityItem.UpdatedDate = DateTime.Now;
                    entityItem.UpdatedBy = _userService.Username;
                    var updatedEntity = await _leaveRepo.UpdateAsync(entityItem);
                    return _mapper.Map<LeaveDto>(updatedEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Leave");
                throw;
            }
        }

        public Task<int> Delete(string code)
        {
            throw new NotImplementedException();
        }
    }
}
