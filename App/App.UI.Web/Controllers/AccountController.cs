using App.Application.Interfaces.Services;
using App.UI.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.UI.Web.Controllers
{
    [AllowAnonymous]
    public sealed class AccountController : BaseController
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        public AccountController(ILogger<AccountController> logger
            , IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = "/";

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserModel model, string returnUrl = "/")
        {
            returnUrl = returnUrl ?? "/";
            _logger.LogWarning($"LoginForm {model?.Username}");

            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model?.Username))
            {
                ModelState.AddModelError("", "Username is required");
                ViewData["ReturnUrl"] = returnUrl;
                return View(model);
            }

            var userValid = await _userService.ValidateUserAsync(model.Username, model.Password);
            if (!userValid)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }
            //TempData["Username"] = model.Username;
            //TempData["ReturnUrl"] = returnUrl;
            await SignInUser(model.Username);

            return Redirect(returnUrl);




            //return RedirectToAction(nameof(Mfa));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Mfa()
        {
            if (TempData["Username"] == null)
                return RedirectToAction(nameof(Login));

            TempData.Keep();

            return View(new OtpModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendOtp(OtpModel model)
        {
            var username = TempData["Username"]?.ToString();
            var returnUrl = TempData["ReturnUrl"]?.ToString() ?? "/";

            if (username == null)
                return RedirectToAction(nameof(Login));

            if (!ModelState.IsValid)
            {
                TempData.Keep();
                return View("Mfa", model);
            }

            var otp = model.GetOtp();

            if (!VerifyOtp(otp, "userSecret"))
            {
                ModelState.AddModelError("", "Invalid or expired code");
                TempData.Keep();
                return View("Mfa", model);
            }

            await SignInUser(username);

            return Redirect(returnUrl);
        }

        private bool VerifyOtp(string otp, object userSecret)
        {
            return true;
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (await _userService.ClearAsync())
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return RedirectToAction(nameof(Login));
        }

        private async Task SignInUser(string username)
        {
            var user = await _userService.GetByKey(username);
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, username), };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
