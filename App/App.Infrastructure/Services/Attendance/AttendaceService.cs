using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Application.Interfaces.Services.Attendance;
using App.Application.Interfaces.Services.Masters;
using App.Domain.Entities;
using App.Domain.Models.Dto.Attendance;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Extensions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Infrastructure.Services.Attendance
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblAttendance> _AttendanceRepo;
        private readonly ILogger<AttendanceService> _logger;
        private readonly IContextService _userService;

        private readonly IGenericRepository<VwAttendance> _vwAttendanceRepo;
        public AttendanceService(IGenericRepository<TblAttendance> AttendanceRepo
            , IGenericRepository<VwAttendance> vwAttendanceRepo
            , IMapper mapper
            , ILogger<AttendanceService> logger
            , IContextService userService)
        {
            _AttendanceRepo = AttendanceRepo;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
            _vwAttendanceRepo = vwAttendanceRepo;
        }

        public async Task<int> DeleteAsync(long id)
        {
            try
            {
                var entityItem = await _AttendanceRepo.FindAsync(t => t.AttendanceId == id)
                      ?? throw new KeyNotFoundException($"Attendance with ID {id} not found");
                return await _AttendanceRepo.Remove(entityItem);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, $"Delete blocked by FK for Attendance ID : {id}");
                throw new Exception(
                    $"Oops, this record (ID: {id}) is used by another configuration and cannot be deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Attendance with ID {id}", id);
                throw;
            }
        }

        public async Task<AttendanceDto> GetByKeyAsync(int Id)
        {   
            try
            {
                var entityItem = await _AttendanceRepo.FindAsync(t => t.AttendanceId == Id);
                return _mapper.Map<AttendanceDto>(entityItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Attendance with Id {Id}", Id);
                throw;
            }
        }

        public async Task<PagedResponse<VwAttendanceDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _vwAttendanceRepo.GetPagedAsync(model);
                return entityResult.MapPaged<VwAttendance, VwAttendanceDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paged Attendance data");
                throw;
            }
        }

        public async Task<List<AttendanceDto>> GetListAsync()
        {
            try
            {
                var entityList = await _AttendanceRepo.GetListAsync();
                return _mapper.Map<List<AttendanceDto>>(entityList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Attendance list");
                throw;
            }
        }

        public async Task<AttendanceDto> SaveAsync(AttendanceDto model)
        {
            try
            {
                var entityItem = await _AttendanceRepo.FindAsync(t => t.AttendanceId.Equals(model.AttendanceId));
                if (entityItem == null)
                {
                    TblAttendance item = _mapper.Map<TblAttendance>(model);
                    item.CreatedDate = DateTime.Now;
                    item.CreatedBy = _userService.Username;
                    var addedEntity = await _AttendanceRepo.AddAsync(item);
                    return _mapper.Map<AttendanceDto>(addedEntity);
                }
                else
                {
                    _mapper.Map(model, entityItem);
                    entityItem.UpdatedDate = DateTime.Now;
                    entityItem.UpdatedBy = _userService.Username;
                    var updatedEntity = await _AttendanceRepo.UpdateAsync(entityItem);
                    return _mapper.Map<AttendanceDto>(updatedEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Attendance");
                throw;
            }
        }
         
    }
}
