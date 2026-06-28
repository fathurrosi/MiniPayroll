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
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblEmployee> _EmployeeRepo;

        private readonly ILogger<EmployeeService> _logger;
        private readonly IContextService _userService;
        public EmployeeService(IGenericRepository<TblEmployee> EmployeeRepo
            , IMapper mapper
            , ILogger<EmployeeService> logger
            , IContextService userService)
        {
            _EmployeeRepo = EmployeeRepo;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
        }

        public async Task<int> Delete(long code)
        {
            try
            {
                var entityItem = await _EmployeeRepo.FindAsync(t => t.EmployeeCode.Equals(code))
                      ?? throw new KeyNotFoundException($"Code {code} not found");
                return await _EmployeeRepo.Remove(entityItem);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, $"Delete blocked by FK for Code : {code}");

                throw new Exception(
                    $"Oops, this record ({code}) is used by another configuration and cannot be deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Employee with code {code}", code);
                throw;
            }
        }

        public async Task<EmployeeDto> GetByCode(string code)
        {
            try
            {
                var entityItem = await _EmployeeRepo.FindAsync(t => t.EmployeeCode == code);
                return _mapper.Map<EmployeeDto>(entityItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged Employee with code {code}", code);
                throw;
            }
        }

        public async Task<PagedResponse<EmployeeDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _EmployeeRepo.GetPagedAsync(model); 
                return entityResult.MapPaged<TblEmployee, EmployeeDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged Employees");
                throw;
            }
        }

        public async Task<List<EmployeeDto>> GetListAsync()
        {
            try
            {
                var entityResult = await _EmployeeRepo.GetListAsync();
                return entityResult.Select(t => _mapper.Map<EmployeeDto>(t)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Employees");
                throw;
            }
        }

        public async Task<EmployeeDto> Save(EmployeeDto model)
        {
            try
            {
                var entityItem = await _EmployeeRepo.FindAsync(t => t.EmployeeCode.Equals(model.EmployeeCode));
                if (entityItem == null)
                {
                    TblEmployee item = _mapper.Map<TblEmployee>(model); 
                    var addedEntity = await _EmployeeRepo.AddAsync(item);
                    return _mapper.Map<EmployeeDto>(addedEntity);
                }
                else
                {
                    _mapper.Map(model, entityItem); 
                    var updatedEntity = await _EmployeeRepo.UpdateAsync(entityItem);
                    return _mapper.Map<EmployeeDto>(updatedEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Employee");
                throw;
            }
        }

        public Task<int> Delete(string code)
        {
            throw new NotImplementedException();
        }
    }

}
