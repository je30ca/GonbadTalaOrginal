using DataAccess.Data;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using AdminGonbadTala.Services;

namespace AdminGonbadTala.Controllers
{
    public class AccountController : Controller
    {
        private readonly GonbadDbContext _context;
        private readonly KhademPasswordService _passwordService;

        public AccountController(GonbadDbContext context, KhademPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        // صفحه ورود (GET)
        [AllowAnonymous]
        public IActionResult Login()
        {
            // اگر قبلاً لاگین کرده، بفرستش به داشبورد یا لیست کودکان
            if (HttpContext.Session.GetInt32("KhademId") != null)
            {
                return RedirectToAction("Index","Timesheets"); 
                    //return RedirectToAction("Login", "Account");
            }
            return View();
            
        }

        // ثبت ورود (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string phoneNumber, string password)
        {
            var khadem = await _context.Khadems
                .FirstOrDefaultAsync(k => k.PhoneNumber == phoneNumber);

            if (khadem != null)
            {
                if (_passwordService.VerifyAndUpgradeIfNeeded(khadem, password))
                {
                    await _context.SaveChangesAsync();
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, khadem.Id.ToString()),
                        new Claim(ClaimTypes.Name, khadem.FullName)
                    };
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

                    // ذخیره اطلاعات خادم در سشن
                    HttpContext.Session.SetInt32("KhademId", khadem.Id);
                    HttpContext.Session.SetString("KhademName", khadem.FirstName+" "+ khadem.LastName);

                    return RedirectToAction("Index", "Timesheets"); // هدایت به صفحه اصلی حضوروغیاب
                }
            }

            ViewBag.Error = "شماره همراه یا رمز عبور اشتباه است.";
            return View();
        }

        // خروج از حساب کاربری
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
