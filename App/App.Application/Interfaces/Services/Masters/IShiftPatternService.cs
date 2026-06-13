using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Interfaces.Services.Masters
{
    public interface IShiftPatternService
    {
        Task<List<ShiftPatternDto>> GetListAsync();
        Task<ShiftPatternDto?> GetByIdAsync(int id);
        Task<int> DeleteAsync(int id);
        Task<PagedResponse<ShiftPatternDto>> GetPagedAsync(DataTableRequest model);
        Task<ShiftPatternDto> SaveAsync(ShiftPatternDto model);
    }
}
