using App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.Domain.Models.Dto.Masters
{
    public class RoleDto : BaseDto<TblRole>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Role code is required.")]
        [StringLength(100, ErrorMessage = "Role code cannot exceed 100 characters.")]
        [Display(Name = "Role Code")]
        public string RoleCode { get; set; }

        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(255, ErrorMessage = "Role name cannot exceed 255 characters.")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        public bool IsDeleted { get; set; }
    }
}
