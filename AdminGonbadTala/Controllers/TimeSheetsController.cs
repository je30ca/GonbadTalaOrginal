using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using DataAccess.Models;

namespace AdminGonbadTala.Controllers
{
    public class TimeSheetsController : Controller
    {
        private readonly GonbadDbContext _context;

        public TimeSheetsController(GonbadDbContext context)
        {
            _context = context;
        }

        // GET: TimeSheets
        //public async Task<IActionResult> Index()
        //{
        //    var gonbadDbContext = _context.TimeSheets.Include(t => t.Childd);
        //    return View(await gonbadDbContext.ToListAsync());
        //}

        // GET: TimeSheets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeSheet = await _context.TimeSheets
                .Include(t => t.Childd)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeSheet == null)
            {
                return NotFound();
            }

            return View(timeSheet);
        }

        // GET: TimeSheets/Create
        public IActionResult Create()
        {
            ViewData["ChilddId"] = new SelectList(_context.Kids, "Id", "FirstName");
            return View();
        }

        // POST: TimeSheets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EntryTime,ExitTime,ChilddId")] TimeSheet timeSheet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(timeSheet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChilddId"] = new SelectList(_context.Kids, "Id", "FirstName", timeSheet.ChilddId);
            return View(timeSheet);
        }

        // GET: TimeSheets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeSheet = await _context.TimeSheets.FindAsync(id);
            if (timeSheet == null)
            {
                return NotFound();
            }
            ViewData["ChilddId"] = new SelectList(_context.Kids, "Id", "FirstName", timeSheet.ChilddId);
            return View(timeSheet);
        }

        // POST: TimeSheets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EntryTime,ExitTime,ChilddId")] TimeSheet timeSheet)
        {
            if (id != timeSheet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeSheet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeSheetExists(timeSheet.Id))
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
            ViewData["ChilddId"] = new SelectList(_context.Kids, "Id", "FirstName", timeSheet.ChilddId);
            return View(timeSheet);
        }

        // GET: TimeSheets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeSheet = await _context.TimeSheets
                .Include(t => t.Childd)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeSheet == null)
            {
                return NotFound();
            }

            return View(timeSheet);
        }

        // POST: TimeSheets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timeSheet = await _context.TimeSheets.FindAsync(id);
            if (timeSheet != null)
            {
                _context.TimeSheets.Remove(timeSheet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Index(string shift = "", string searchString = "")
        {
            var today = DateTime.Today;
            var now = DateTime.Now;

            // تشخیص شیفت فعلی بر اساس ساعت
            if (string.IsNullOrEmpty(shift))
            {
                shift = (now.Hour < 15) ? "morning" : "evening";
            }

            var query = _context.TimeSheets
                .Include(t => t.Childd)
                .Where(t => t.EntryTime.Date == today); // فقط برای امروز

            // فیلتر بر اساس شیفت
            if (shift == "morning")
            {
                query = query.Where(t => t.EntryTime.Hour < 14);
            }
            else
            {
                query = query.Where(t => t.EntryTime.Hour >= 14);
            }

            // 🔴 افزودن فیلتر جستجو بر اساس نام، نام خانوادگی یا شماره تماس
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.Trim();
                query = query.Where(t =>
                    t.Childd.FirstName.Contains(searchString) ||
                    t.Childd.LastName.Contains(searchString) ||
                    (t.Childd.FirstName + " " + t.Childd.LastName).Contains(searchString) ||
                    t.Childd.PhoneNumber.Contains(searchString)
                );
            }

            ViewBag.CurrentShift = shift;
            ViewData["CurrentFilter"] = searchString; // ذخیره عبارت جستجو برای لود مجدد در فرم

            var list = await query
                .OrderBy(t => t.ExitTime.HasValue)
                .ThenByDescending(t => t.EntryTime)
                .ToListAsync();

            ViewBag.TotalCount = list.Count;
            ViewBag.TravelerCount = list.Count(t => t.Childd.IsTraveler);
            ViewBag.RegularCount = list.Count(t => !t.Childd.IsTraveler);
            ViewBag.PresentCount = list.Count(t => t.ExitTime == null);

            return View(list);
        }

        //public async Task<IActionResult> Index(string shift = "")
        //{
        //    var today = DateTime.Today;
        //    var now = DateTime.Now;

        //    // تشخیص شیفت فعلی بر اساس ساعت (مثلا 14 مرز بین صبح و عصر)
        //    if (string.IsNullOrEmpty(shift))
        //    {
        //        shift = (now.Hour < 15) ? "morning" : "evening";
        //    }

        //    var query = _context.TimeSheets
        //        .Include(t => t.Childd)
        //        .Where(t => t.EntryTime.Date == today); // فقط برای امروز

        //    // فیلتر بر اساس شیفت
        //    if (shift == "morning")
        //    {
        //        query = query.Where(t => t.EntryTime.Hour < 14);
        //    }
        //    else
        //    {
        //        query = query.Where(t => t.EntryTime.Hour >= 14);
        //    }

        //    ViewBag.CurrentShift = shift;

        //    var list = await query
        //        .OrderBy(t => t.ExitTime.HasValue)
        //        .ThenByDescending(t => t.EntryTime)
        //        .ToListAsync();
        //    ViewBag.TotalCount = list.Count;
        //    ViewBag.TravelerCount = list.Count(t => t.Childd.IsTraveler);
        //    ViewBag.RegularCount = list.Count(t => !t.Childd.IsTraveler);

        //    ViewBag.PresentCount = list.Count(t => t.ExitTime == null);

        //    return View(list);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(int id)
        {
            var timeSheet = await _context.TimeSheets.FindAsync(id);
            if (timeSheet == null)
            {
                return NotFound();
            }

            // ثبت زمان خروج
            timeSheet.ExitTime = DateTime.Now;

            _context.Update(timeSheet);
            await _context.SaveChangesAsync();

            // هدایت به صفحه لیست (ایندکس)
            return RedirectToAction(nameof(Index));
        }


        private bool TimeSheetExists(int id)
        {
            return _context.TimeSheets.Any(e => e.Id == id);
        }
    }
}
