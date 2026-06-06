using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Domain.Models.Dto
{
    public sealed class PtkpDto : BaseDto<TblPtkp>
    {
        public string Ptkpcode { get; set; } = null!;
         
        public string Ptkpname { get; set; } = null!;
         
        public decimal NominalYearly { get; set; }
         
        public decimal NominalMonthly { get; set; }
         
        public string Tercategory { get; set; } = null!;

        public bool IsActive { get; set; }
         
    }
}
