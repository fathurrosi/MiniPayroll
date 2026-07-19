using System;
using System.Collections.Generic;
using System.Text;

namespace App.Domain.Models.Dto.Settings
{
    public class MenuPermissionDto
    {
        public long MenuId { get; set; }

        public string MenuName { get; set; } = "";

        public int Level { get; set; }

        public bool HasChildren { get; set; }

        public bool CanView { get; set; }

        public bool CanCreate { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool CanExport { get; set; }

        public bool CanApprove { get; set; }
    }
}
