using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using DataAccess.Models;
using AdminGonbadTala.Services;

namespace AdminGonbadTala.Controllers
{
    [Authorize(Roles = UserRoles.Management + "," + UserRoles.ShiftLead)]
    public class KhademsController : Controller
    {
        private readonly GonbadDbContext _context;
        private readonly KhademPasswordService _passwordService;

        public KhademsController(GonbadDbContext context, KhademPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        // GET: Khadems
        public async Task<IActionResult> Index()
        {
            return View(await _context.Khadems.ToListAsync());
        }

        // GET: Khadems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khadem = await _context.Khadems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (khadem == null)
            {
                return NotFound();
            }

            return View(khadem);
        }

        // GET: Khadems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Khadems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string FullName, string password, [Bind("Id,FirstName,PersonalCode,LastName,PhoneNumber,Specialization,WorkingDay,Shift,Role")] Khadem khadem)
        {
            // ۱. بررسی اینکه نام کامل خالی نباشد و فاصله‌های اضافه دور ریخته شوند
            if (!string.IsNullOrWhiteSpace(FullName))
            {
                var cleanedName = FullName.Trim();
                var parts = cleanedName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length > 0)
                {
                    // کلمه اول همیشه نام است
                    khadem.FirstName = parts[0];

                    if (parts.Length > 1)
                    {
                        // تمام بخش‌های بعدی به عنوان فامیل به هم چسبانده می‌شوند
                        khadem.LastName = string.Join(" ", parts.Skip(1));
                    }
                    else
                    {
                        // اگر فامیل وارد نکرده بود، خالی نماند
                        khadem.LastName = "-";
                    }
                }
            }
            // حذف فیلدهای ناوبری و فیلدهایی که در فرم حضور ندارند از لیست اعتبارسنجی
            ModelState.Remove("TimeSheets");
            ModelState.Remove("RegisteredByKhademId");
            ModelState.Remove("RegisteredByKhadem");
            ModelState.Remove("Subordinates"); // اگر رابطه‌ای به خود خادم دارد
            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");

            if (string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("password", "لطفاً رمز عبور را وارد کنید");
            }

            if (ModelState.IsValid)
            {
                if (!UserRoles.All.Contains(khadem.Role)) khadem.Role = UserRoles.Servant;
                _passwordService.SetPassword(khadem, password);
                _context.Add(khadem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khadem);
        }

        // GET: Khadems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khadem = await _context.Khadems.FindAsync(id);
            if (khadem == null)
            {
                return NotFound();
            }
            return View(khadem);
        }

        // POST: Khadems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string? password, [Bind("Id,FirstName,LastName,PersonalCode,PhoneNumber,Specialization,WorkingDay,Shift,Role")] Khadem khadem)
        {
            if (id != khadem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingKhadem = await _context.Khadems.FindAsync(id);
                    if (existingKhadem == null)
                    {
                        return NotFound();
                    }

                    existingKhadem.FirstName = khadem.FirstName;
                    existingKhadem.LastName = khadem.LastName;
                    existingKhadem.PersonalCode = khadem.PersonalCode;
                    existingKhadem.PhoneNumber = khadem.PhoneNumber;
                    existingKhadem.Specialization = khadem.Specialization;
                    existingKhadem.WorkingDay = khadem.WorkingDay;
                    existingKhadem.Shift = khadem.Shift;
                    existingKhadem.Role = UserRoles.All.Contains(khadem.Role) ? khadem.Role : UserRoles.Servant;

                    if (!string.IsNullOrWhiteSpace(password))
                    {
                        _passwordService.SetPassword(existingKhadem, password);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhademExists(khadem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(khadem);
        }

        // GET: Khadems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khadem = await _context.Khadems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (khadem == null)
            {
                return NotFound();
            }

            return View(khadem);
        }

        // POST: Khadems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khadem = await _context.Khadems.FindAsync(id);
            if (khadem != null)
            {
                _context.Khadems.Remove(khadem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhademExists(int id)
        {
            return _context.Khadems.Any(e => e.Id == id);
        }
    }
}
