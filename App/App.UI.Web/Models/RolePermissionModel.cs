namespace App.UI.Web.Models
{
    public sealed class RolePermissionModel
    {
        public string RoleCode { get; set; } = string.Empty;

        public string MenuId { get; set; } = string.Empty;

        public bool CanView { get; set; }

        public bool CanCreate { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool CanExport { get; set; }

        public bool CanApprove { get; set; }
    }
}
