 
//namespace App.Infrastructure.Extensions
//{
//    public static class SelectableDisplayExtensions
//    {
//        public static string ToCode(
//           this ISelectableDisplay item)
//        {
//            if (item == null)
//                return string.Empty;

//            return item.Code;
//        }
//        public static string ToDisplayText(
//            this ISelectableDisplay item,
//            DisplayFormat format)
//        {
//            if (item == null)
//                return string.Empty;

//            return format switch
//            {
//                DisplayFormat.Code =>
//                    item.Code,

//                DisplayFormat.Name =>
//                    item.Name,

//                //DisplayFormat.CodeAndName =>
//                //    $"{item.Code} - {item.Name}",

//                DisplayFormat.Description =>
//                    item.Description ?? item.Name,

//                _ =>
//                    item.Name
//            };
//        }
//    }
//}
