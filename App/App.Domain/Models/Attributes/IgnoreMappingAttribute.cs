using System;
using System.Collections.Generic;
using System.Text;

namespace App.Domain.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreMappingAttribute : Attribute { }
}
