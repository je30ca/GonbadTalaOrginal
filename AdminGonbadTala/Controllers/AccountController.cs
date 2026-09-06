using DataAccess.Data;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace AdminGonbadTala.Controllers
{
    public class AccountController : Controller
    {
        private readonly GonbadDbContext _context;
        private readonly IPasswordHasher<Khadem> _passwordHasher;

        public AccountController(GonbadDbContext context, IPasswordHasher<Khadem> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
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
                PasswordVerificationResult verification;
                try
                {
                    verification = _passwordHasher.VerifyHashedPassword(
                        khadem, khadem.PasswordHash, password);
                }
                catch (FormatException)
                {
                    verification = PasswordVerificationResult.Failed;
                }

                // PasswordHash contains legacy plain-text values immediately after
                // the migration. Upgrade a matching legacy value on first login.
                if (verification == PasswordVerificationResult.Failed &&
                    IsLegacyPasswordMatch(khadem.PasswordHash, password))
                {
                    khadem.PasswordHash = _passwordHasher.HashPassword(khadem, password);
                    await _context.SaveChangesAsync();
                    verification = PasswordVerificationResult.Success;
                }

                if (verification != PasswordVerificationResult.Failed)
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
            }

            ViewBag.Error = "شماره همراه یا رمز عبور اشتباه است.";
            return View();
        }

        private static bool IsLegacyPasswordMatch(string storedValue, string suppliedPassword)
        {
            if (string.IsNullOrEmpty(storedValue))
            {
                return false;
            }

            var storedBytes = Encoding.UTF8.GetBytes(storedValue);
            var suppliedBytes = Encoding.UTF8.GetBytes(suppliedPassword);
            return storedBytes.Length == suppliedBytes.Length &&
                   CryptographicOperations.FixedTimeEquals(storedBytes, suppliedBytes);
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
