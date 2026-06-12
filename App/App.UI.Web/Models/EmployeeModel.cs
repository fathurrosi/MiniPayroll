using App.Domain.Models;
using App.Domain.Models.Dto;

namespace App.UI.Web.Models
{
    public sealed class EmployeeModel : PageModel<EmployeeDto>
    {
        public List<PtkpDto> PtkpList { get; set; }
    }
}
