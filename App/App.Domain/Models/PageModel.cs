
using App.Domain.Enums;

namespace App.Domain.Models
{
    public class PageModel<Dto>
    {
        public string? Type { get; set; }
        public string? Title { get; set; }
        public Dto Item { get; set; }
        public FormMode Mode { get; set; } = FormMode.Create;
    }
}
