using System;
using System.Collections.Generic;
using System.Text;

namespace App.Domain.Models.Request
{
    public sealed class GenerateScheduleRequest
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public string DepartmentCode { get; set; }

        //public string Pattern { get; set; }
        public int PatternId { get; set; }
        public bool OverwriteExisting { get; set; }
    }
}
