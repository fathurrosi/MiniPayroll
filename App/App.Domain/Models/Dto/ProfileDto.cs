using App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Domain.Models.Dto
{
    public class ProfileDto : BaseDto<TblCompanyProfile>
    {
        public int CompanyProfileId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string CompanyAddress { get; set; } = string.Empty;

        public string? LogoFileName { get; set; }

        public string? LogoFilePath { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Website { get; set; }

        public string? TaxNumber { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
