//using App.Domain.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace App.Domain.Handlers
//{

//    public class CookieHandler : CookieAuthenticationEvents
//    {
//        public override Task SignedIn(CookieSignedInContext context)
//        {
//            return base.SignedIn(context);
//        }

//        public override Task SigningIn(CookieSigningInContext context)
//        {
//            return base.SigningIn(context);
//        }

//        public override Task ValidatePrincipal(CookieValidatePrincipalContext context)
//        {

//            if (context.Principal?.Identity?.IsAuthenticated == false)
//            {
//                context.RejectPrincipal();
//                context.ShouldRenew = true;
//                return Task.CompletedTask;
//            }


//            string? userName = context.Principal?.Identity?.Name;
//            if (context.HttpContext.Request.Cookies.TryGetValue("Payroll", out string protectedUserData))
//            {
//                //UserDto userData = JsonConvert.DeserializeObject<UserDto>(Encoding.UTF8.GetString(Convert.FromBase64String(protectedUserData)));
//            }
//            return base.ValidatePrincipal(context);
//        }

//        //public override Task RedirectToReturnUrl(RedirectContext<CookieAuthenticationOptions> context)
//        //{
//        //    return base.RedirectToReturnUrl(context);
//        //}
//    }
//}
