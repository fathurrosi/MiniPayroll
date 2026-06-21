using App.Domain.Models.Dto.Payroll;
using App.Domain.Models.Request;
using App.Domain.Models.Response;

namespace App.Application.Interfaces.Services.Masters
{
    public interface IPayslipService
    {
        Task<List<PayslipDto>> GetListAsync();
        Task<PayslipDto?> GetByIdAsync(int id);  
        Task<int> DeleteAsync(int id);
        Task<PagedResponse<PayslipDto>> GetPagedAsync(DataTableRequest model); 
        Task<PayslipDto> SaveAsync(PayslipDto model);  
    }
}
