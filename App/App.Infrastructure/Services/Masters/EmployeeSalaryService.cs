using App.Application.Interfaces.Procedures;
using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Application.Interfaces.Services.Masters;
using App.Domain.Entities;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Extensions;
using Dapper;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace App.Infrastructure.Services.Masters
{
    public class EmployeeSalaryService : IEmployeeSalaryService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblEmployeeSalary> _EmployeeSalaryRepo;
        private readonly IGenericRepository<TblEmployeeSalaryDetail> _EmployeeSalaryDetailRepo;
        private readonly IGenericRepository<VwEmployeeSalary> _VwEmployeeSalaryRepo;
        private readonly IGenericRepository<TblSalaryComponent> _TblSalaryComponentRepo;
        private readonly ILogger<EmployeeSalaryService> _logger;
        private readonly IProcedureExecutor _executor;
        private readonly IContextService _userService;
        public EmployeeSalaryService(IGenericRepository<TblEmployeeSalary> EmployeeSalaryRepo
            , IGenericRepository<VwEmployeeSalary> VwEmployeeSalaryRepo
            , IGenericRepository<TblSalaryComponent> TblSalaryComponentRepo
            , IProcedureExecutor executor
            , IGenericRepository<TblEmployeeSalaryDetail> EmployeeSalaryDetailRepo
            , IMapper mapper
            , ILogger<EmployeeSalaryService> logger
            , IContextService userService)
        {
            _executor = executor;
            _EmployeeSalaryRepo = EmployeeSalaryRepo;
            _VwEmployeeSalaryRepo = VwEmployeeSalaryRepo;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
            _TblSalaryComponentRepo = TblSalaryComponentRepo;
            _EmployeeSalaryDetailRepo = EmployeeSalaryDetailRepo;
        }


        public async Task<int> DeleteAsync(int code)
        {
            try
            {
                int employeeId = code;


                var entityItem = await _EmployeeSalaryRepo.FindAsync(t => t.EmployeeId == employeeId)
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

        public async Task<EmployeeSalaryDto> GetByCodeAsync(int code)
        {
            try
            {
                int employeeId = code;

                var entityItem = await _EmployeeSalaryRepo.FindAsync(t => t.EmployeeId == employeeId);
                return _mapper.Map<EmployeeSalaryDto>(entityItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged EmployeeSalary with code {code}", code);
                throw;
            }
        }

        public async Task<PagedResponse<VwEmployeeSalaryDto>> GetPagedAsync(EmployeeSalaryDataTableRequest model)
        {
            try
            {
                int employeeId = 0;
                int.TryParse(model.EmployeeId, out employeeId);

                var entityResult = await _VwEmployeeSalaryRepo.GetPagedAsync(
                                t =>
                                    (employeeId == 0 || t.EmployeeId == employeeId) &&
                                    (string.IsNullOrWhiteSpace(model.PositionCode) || t.Position == model.PositionCode) &&
                                    (string.IsNullOrWhiteSpace(model.DepartmentCode) || t.Department == model.DepartmentCode),
                                model);

                var results = entityResult.MapPaged<VwEmployeeSalary, VwEmployeeSalaryDto>(_mapper, model);

                var salaryComponents = await _TblSalaryComponentRepo.GetListAsync();
                var orderedComponents = salaryComponents.OrderBy(c => c.SortOrder).ToList();
                var details = await _EmployeeSalaryDetailRepo.GetListAsync();
                for (int i = 0; i < results.Items.Count; i++)
                {
                    var result = results.Items[i];
                    var employeeDetails = details.Where(d => d.EmployeeId == result.EmployeeId).ToList();
                    foreach (var c in orderedComponents)
                    {
                        decimal amount = employeeDetails.FirstOrDefault(d => d.ComponentCode == c.ComponentCode)?.Amount ?? 0;
                        result.Components[c.ComponentCode] = amount;
                    }
                }


                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged EmployeeSalarys");
                throw;
            }
        }



        public async Task<List<VwEmployeeSalaryDto>> GetListAsync(string department, string position, int employeeId)
        {
            try
            {
                var entityResult = await _VwEmployeeSalaryRepo.GetListAsync(t =>
                (t.Department == department || string.IsNullOrEmpty(department)) &&
                     (t.Position == position || string.IsNullOrEmpty(position)) &&
                     (t.EmployeeId == employeeId || employeeId == 0)

                );
                return entityResult.Select(t => _mapper.Map<VwEmployeeSalaryDto>(t)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting EmployeeSalarys");
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
                var entityItem = await _EmployeeSalaryRepo.FindAsync(t => t.EmployeeId == model.EmployeeId);
                if (entityItem == null)
                {
                    TblEmployeeSalary item = _mapper.Map<TblEmployeeSalary>(model);
                    item.CreatedBy = _userService.Username;
                    item.CreatedDate = DateTime.UtcNow;
                    var addedEntity = await _EmployeeSalaryRepo.AddAsync(item);
                    return _mapper.Map<EmployeeSalaryDto>(addedEntity);
                }
                else
                {
                    _mapper.Map(model, entityItem);
                    entityItem.UpdatedBy = _userService.Username;
                    entityItem.UpdatedDate = DateTime.UtcNow;
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


        public async Task<bool> SaveAsync(List<EmployeeSalaryDto> items, List<EmployeeSalaryDetailDto> details)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeSalaries",
                    CreateEmployeeSalaryTable(items).AsTableValuedParameter("type_EmployeeSalary"));

                parameters.Add("@EmployeeSalaryDetails",
                    CreateEmployeeSalaryDetailTable(details).AsTableValuedParameter("type_EmployeeSalaryDetail"));

                await _executor.ExecuteNonQueryAsync("sp_SaveEmployeeSalary", parameters);

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving EmployeeSalary");
                throw;
            }
        }

        private DataTable CreateEmployeeSalaryTable(List<EmployeeSalaryDto> items)
        {
            var table = new DataTable();
            table.Columns.Add("EmployeeId", typeof(int));
            table.Columns.Add("EffectiveDate", typeof(DateTime));
            table.Columns.Add("IsActive", typeof(bool));
            table.Columns.Add("CreatedDate", typeof(DateTime));
            table.Columns.Add("UpdatedDate", typeof(DateTime));
            table.Columns.Add("CreatedBy", typeof(string));
            table.Columns.Add("UpdatedBy", typeof(string));

            foreach (var item in items)
            {
                table.Rows.Add(
                    item.EmployeeId,
                    item.EffectiveDate,
                    item.IsActive,
                    item.CreatedDate.HasValue ? (object)item.CreatedDate.Value : DateTime.UtcNow,
                    item.UpdatedDate.HasValue ? (object)item.UpdatedDate.Value : DateTime.UtcNow,
                    $"{_userService.Username}",
                    $"{_userService.Username}"
                    );
            }

            return table;
        }

        private DataTable CreateEmployeeSalaryDetailTable(List<EmployeeSalaryDetailDto> items)
        {
            var table = new DataTable();
            table.Columns.Add("EmployeeId", typeof(int));
            table.Columns.Add("ComponentCode", typeof(string));
            table.Columns.Add("Amount", typeof(decimal));
            table.Columns.Add("CreatedDate", typeof(DateTime));
            table.Columns.Add("UpdatedDate", typeof(DateTime));
            table.Columns.Add("CreatedBy", typeof(string));
            table.Columns.Add("UpdatedBy", typeof(string));

            foreach (var item in items)
            {
                table.Rows.Add(
                    item.EmployeeId,
                    item.ComponentCode,
                    item.Amount,
                    item.CreatedDate.HasValue ? (object)item.CreatedDate.Value : DateTime.UtcNow,
                    item.UpdatedDate.HasValue ? (object)item.UpdatedDate.Value : DateTime.UtcNow,
                    $"{_userService.Username}",
                    $"{_userService.Username}"
                    );
            }

            return table;
        }

    }

}
