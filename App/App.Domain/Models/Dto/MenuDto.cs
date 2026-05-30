using App.Domain.Entities;
using System.ComponentModel.DataAnnotations;


namespace App.Domain.Models.Dto
{
    public sealed class MenuDto : BaseDto<TblMenu>
    {
        [Key]
        [MaxLength(255)]
        public string Id { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string Caption { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string Link { get; set; } = null!;

        [MaxLength(255)]
        public string? Icon { get; set; }

        public int Sort { get; set; }

        [MaxLength(255)]
        public string? Css { get; set; }

        [MaxLength(255)]
        public string? MenuTitle { get; set; }

        [MaxLength(255)]
        public string? ParentId { get; set; }

        public List<MenuDto> ChildList { get; set; }
    }
}
