using App.Domain.Entities;
 
namespace App.Domain.Models.Dto.Settings
{
    public sealed class UserDto : BaseDto<TblUser>
    {
        public long Id { get; set; }
        public string Username { get; set; } = null!;
        public string? Email { get; set; }
        public string PasswordHash { get; set; } = null!;
        public string? FullName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public List<MenuDto> MenuList { get; set; }
    }
}
