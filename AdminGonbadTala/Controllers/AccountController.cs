using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace AdminGonbadTala.Controllers
{
    public class AccountController : Controller
    {
        private readonly GonbadDbContext _context;

        public AccountController(GonbadDbContext context)
        {
            _context = context;
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
                .FirstOrDefaultAsync(k => k.PhoneNumber == phoneNumber && k.Password == password );

            if (khadem != null)
            {
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
