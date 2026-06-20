using App.Domain.Entities;
namespace App.Domain.Models.Dto.Masters
{
    public sealed class PositionDto : BaseDto<TblPosition>
    {
        public string PositionCode { get; set; }
        public string? PositionName { get; set; }

        public string? GradeLevel { get; set; }

    }
}
