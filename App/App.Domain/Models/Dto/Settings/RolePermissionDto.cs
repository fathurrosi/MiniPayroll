using App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Domain.Models.Dto.Settings
{
    public sealed class RolePermissionDto : BaseDto<TblRolePermission>
    {
        public long Id { get; set; }

        public string RoleCode { get; set; }

        public string MenuId { get; set; }

        public bool CanView { get; set; }

        public bool CanCreate { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool CanExport { get; set; }

        public bool CanApprove { get; set; }

        public bool IsDeleted { get; set; }
    }
}
