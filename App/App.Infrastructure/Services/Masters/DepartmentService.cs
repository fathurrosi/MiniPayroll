using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Application.Interfaces.Services.Masters;
using App.Domain.Entities;
using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Extensions; 
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace App.Infrastructure.Services.Masters
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblDepartment> _DepartmentRepo;
        private readonly ILogger<DepartmentService> _logger;
        private readonly IContextService _userService;
        public DepartmentService(IGenericRepository<TblDepartment> DepartmentRepo
            , IMapper mapper
            , ILogger<DepartmentService> logger
            , IContextService userService)
        {
            _DepartmentRepo = DepartmentRepo;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
        }

        public async Task<int> Delete(long code)
        {
            try
            {
                var entityItem = await _DepartmentRepo.FindAsync(t => t.DepartmentCode.Equals(code))
                      ?? throw new KeyNotFoundException($"Code {code} not found");
                return await _DepartmentRepo.Remove(entityItem);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, $"Delete blocked by FK for Code : {code}");

                throw new Exception(
                    $"Oops, this record ({code}) is used by another configuration and cannot be deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Department with code {code}", code);
                throw;
            }
        }

        public async Task<DepartmentDto> GetByCode(string code)
        {
            try
            {
                var entityItem = await _DepartmentRepo.FindAsync(t => t.DepartmentCode == code);
                return _mapper.Map<DepartmentDto>(entityItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged Department with code {code}", code);
                throw;
            }
        }

        public async Task<PagedResponse<DepartmentDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _DepartmentRepo.GetPagedAsync(model); 
                return entityResult.MapPaged<TblDepartment, DepartmentDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged Departments");
                throw;
            }
        }

        public async Task<List<DepartmentDto>> GetListAsync()
        {
            try
            {
                var entityResult = await _DepartmentRepo.GetListAsync();
                return entityResult.Select(t => _mapper.Map<DepartmentDto>(t)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Departments");
                throw;
            }
        }

        public async Task<DepartmentDto> Save(DepartmentDto model)
        {
            try
            {
                var entityItem = await _DepartmentRepo.FindAsync(t => t.DepartmentCode.Equals(model.DepartmentCode));
                if (entityItem == null)
                {
                    TblDepartment item = _mapper.Map<TblDepartment>(model);
                    item.CreatedDate = DateTime.Now;
                    item.CreatedBy = _userService.Username;
                    var addedEntity = await _DepartmentRepo.AddAsync(item);
                    return _mapper.Map<DepartmentDto>(addedEntity);
                }
                else
                {
                    _mapper.Map(model, entityItem);
                    entityItem.CreatedDate = DateTime.Now;
                    entityItem.CreatedBy = _userService.Username;
                    var updatedEntity = await _DepartmentRepo.UpdateAsync(entityItem);
                    return _mapper.Map<DepartmentDto>(updatedEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Department");
                throw;
            }
        }

        public Task<int> Delete(string code)
        {
            throw new NotImplementedException();
        }
    }

}
