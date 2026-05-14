 
using System.ComponentModel;
using System.Reflection;

namespace App.Infrastructure.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }
        //public static List<EnumDto> ToList<TEnum>() where TEnum : Enum
        //{
        //    return Enum.GetValues(typeof(TEnum))
        //        .Cast<TEnum>()
        //        .Select(e => new EnumDto
        //        {
        //            Name = e.ToString(),
        //            Description = GetDescription(e)
        //        })
        //        .ToList();
        //}
    }
}
