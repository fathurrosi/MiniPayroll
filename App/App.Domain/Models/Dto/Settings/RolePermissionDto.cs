using App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Domain.Models.Dto.Settings
{



    public sealed class VwRolePermissionDto : BaseDto<VwRolePermission>
    {

        public long? Id { get; set; }
        public string RoleCode { get; set; }
        public string MenuId { get; set; }
        public bool? CanView { get; set; }
        public bool? CanCreate { get; set; }
        public bool? CanEdit { get; set; }
        public bool? CanDelete { get; set; }
        public bool? CanExport { get; set; }
        public bool? CanApprove { get; set; }
        public bool? IsDeleted { get; set; }
        public string RoleName { get; set; }
        public int? RoleLevel { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public string ParentId { get; set; }
        public int MenuLevel { get; set; }

        public bool HasChildren
        {
            get
            {
                if (string.IsNullOrEmpty(ParentId)) return true;
                return false;
            }
        }

    }
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
