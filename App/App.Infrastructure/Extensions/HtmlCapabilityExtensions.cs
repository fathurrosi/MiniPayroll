
 
//using Microsoft.AspNetCore.Mvc.Rendering;
//using System.Security.Claims;

//namespace App.Infrastructure.Extensions
//{
//    public static class HtmlCapabilityExtensions
//    {
//        private static CapabilityInfo? GetCapabilityInfo(IHtmlHelper html)
//        {
//            var capability = html.ViewContext.HttpContext.User
//                .FindFirstValue(ClaimTypes.Role);

//            if (string.IsNullOrEmpty(capability))
//                return null;

//            return CapabilityMatrix.GetMatrix()
//                .FirstOrDefault(m => m.ClientCapability.GetDescription() == capability);
//        }
//        /*
//        @if(Html.CanRead())
//        {
//            < button > Add </ button >
//        }
//        */
//        public static bool CanRead(this IHtmlHelper html)
//        {
//            var info = GetCapabilityInfo(html);
//            return info != null && (info.IsSuperAdmin || info.CanRead);
//        }

//        public static bool CanCreate(this IHtmlHelper html)
//        {
//            var info = GetCapabilityInfo(html);
//            return info != null && (info.IsSuperAdmin || info.CanAdd);
//        }

//        public static bool CanEdit(this IHtmlHelper html)
//        {
//            var info = GetCapabilityInfo(html);
//            return info != null && (info.IsSuperAdmin || info.CanEdit);
//        }

//        public static bool CanDelete(this IHtmlHelper html)
//        {
//            var info = GetCapabilityInfo(html);
//            return info != null && (info.IsSuperAdmin || info.CanDelete);
//        }

//        // Optional generic version
//        /*
//        @if (Html.Can("Create", "Edit"))
//        {
//            <button>SaveAsync</button>
//        }
//         */
//        public static bool Can(this IHtmlHelper html, params string[] actions)
//        {
//            var info = GetCapabilityInfo(html);
//            if (info == null) return false;

//            if (info.IsSuperAdmin) return true;

//            return actions.Any(t =>
//                t == "Read" && info.CanRead ||
//                t == "Create" && info.CanAdd ||
//                t == "Edit" && info.CanEdit ||
//                t == "Delete" && info.CanDelete);
//        }
//    }
//}