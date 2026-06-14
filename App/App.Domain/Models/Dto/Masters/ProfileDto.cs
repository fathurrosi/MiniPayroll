using App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.Domain.Models.Dto.Masters
{
    //public class ProfileDto : BaseDto<TblCompanyProfile>
    //{
    //    public int CompanyProfileId { get; set; }

    //    public string CompanyName { get; set; } = string.Empty;

    //    public string CompanyAddress { get; set; } = string.Empty;

    //    public string? LogoFileName { get; set; }

    //    public string? LogoFilePath { get; set; }

    //    public string? PhoneNumber { get; set; }

    //    public string? Email { get; set; }

    //    public string? Website { get; set; }

    //    public string? TaxNumber { get; set; }

    //    public bool IsActive { get; set; } = true;
    //}

    public class ProfileDto : BaseDto<TblCompanyProfile>
    {
        public int CompanyProfileId { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string CompanyAddress { get; set; }

        public string? LogoFileName { get; set; }
        public string? LogoFilePath { get; set; }

        public string? PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Website { get; set; }

        public string? TaxNumber { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
