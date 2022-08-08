using CookieAuth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CookieAuth.Controllers
{
    public class AccountController : Controller
    {
        public User user = null;
        public AccountController()
        {
            user = new User(1, "hammad", "123456");
        }

        public IActionResult Login(string ReturnUrl = "/")
        {
            LoginModel obj = new LoginModel();
            obj.ReturnUrl = ReturnUrl;
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel obj)
        {
            if (ModelState.IsValid)
            {
                if (user.Username != obj.UserName || user.Password != obj.Password)
                {
                    ViewBag.Message = "Invalid credentials";
                    return View(obj);
                }
                else
                {
                    var claims = new List<Claim>();

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        principal, new AuthenticationProperties() { IsPersistent = obj.RememberLogin });

                    return LocalRedirect(obj.ReturnUrl);
                }
            }
            return View(obj);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }
    }
}
