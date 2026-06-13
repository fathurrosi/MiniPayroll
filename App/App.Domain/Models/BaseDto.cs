
using App.Domain.Models.Attributes;

namespace App.Domain.Models
{
    public abstract class BaseDto<TEntity>
    {
        [IgnoreMapping]
        public DateTime? CreatedDate { get; set; }
        [IgnoreMapping]
        public DateTime? UpdatedDate { get; set; }
        [IgnoreMapping]
        public string? CreatedBy { get; set; }
        [IgnoreMapping]
        public string? UpdatedBy { get; set; }

    }

}
