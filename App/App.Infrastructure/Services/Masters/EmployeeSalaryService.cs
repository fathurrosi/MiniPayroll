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
    public class EmployeeSalaryService : IEmployeeSalaryService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblEmployeeSalary> _EmployeeSalaryRepo;
        private readonly ILogger<EmployeeSalaryService> _logger;
        private readonly IContextService _userService;
        public EmployeeSalaryService(IGenericRepository<TblEmployeeSalary> EmployeeSalaryRepo
            , IMapper mapper
            , ILogger<EmployeeSalaryService> logger
            , IContextService userService)
        {
            _EmployeeSalaryRepo = EmployeeSalaryRepo;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
        }


        public async Task<int> DeleteAsync(string code)
        {
            try
            {
                Guid EmployeeSalaryCode;
                Guid.TryParse(code, out EmployeeSalaryCode);


                var entityItem = await _EmployeeSalaryRepo.FindAsync(t => t.EmployeeSalaryId == EmployeeSalaryCode)
                      ?? throw new KeyNotFoundException($"Code {code} not found");
                return await _EmployeeSalaryRepo.Remove(entityItem);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, $"Delete blocked by FK for Code : {code}");

                throw new Exception(
                    $"Oops, this record ({code}) is used by another configuration and cannot be deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting EmployeeSalary with code {code}", code);
                throw;
            }
        }

        public async Task<EmployeeSalaryDto> GetByCodeAsync(string code)
        {
            try
            {
                Guid EmployeeSalaryCode;
                Guid.TryParse(code, out EmployeeSalaryCode);

                var entityItem = await _EmployeeSalaryRepo.FindAsync(t => t.EmployeeSalaryId == EmployeeSalaryCode);
                return _mapper.Map<EmployeeSalaryDto>(entityItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged EmployeeSalary with code {code}", code);
                throw;
            }
        }

        public async Task<PagedResponse<EmployeeSalaryDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _EmployeeSalaryRepo.GetPagedAsync(model);
                return entityResult.MapPaged<TblEmployeeSalary, EmployeeSalaryDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged EmployeeSalarys");
                throw;
            }
        }

        public async Task<List<EmployeeSalaryDto>> GetListAsync()
        {
            try
            {
                var entityResult = await _EmployeeSalaryRepo.GetListAsync();
                return entityResult.Select(t => _mapper.Map<EmployeeSalaryDto>(t)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting EmployeeSalarys");
                throw;
            }
        }

        public async Task<EmployeeSalaryDto> SaveAsync(EmployeeSalaryDto model)
        {
            try
            {
                var entityItem = await _EmployeeSalaryRepo.FindAsync(t => t.EmployeeSalaryId == model.EmployeeSalaryId);
                if (entityItem == null)
                {
                    TblEmployeeSalary item = _mapper.Map<TblEmployeeSalary>(model);
                    var addedEntity = await _EmployeeSalaryRepo.AddAsync(item);
                    return _mapper.Map<EmployeeSalaryDto>(addedEntity);
                }
                else
                {
                    _mapper.Map(model, entityItem);
                    var updatedEntity = await _EmployeeSalaryRepo.UpdateAsync(entityItem);
                    return _mapper.Map<EmployeeSalaryDto>(updatedEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving EmployeeSalary");
                throw;
            }
        }

    }

}
