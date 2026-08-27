using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using DataAccess.Models;
using PersianDate.Standard;

namespace AdminGonbadTala.Controllers
{
    public class ShiftReportsController : Controller
    {
        private readonly GonbadDbContext _context;

        public ShiftReportsController(GonbadDbContext context)
        {
            _context = context;
        }

        // GET: ShiftReports
        public async Task<IActionResult> Index()
        {
            var gonbadDbContext = _context.ShiftReports.Include(s => s.Khadem);
            return View(await gonbadDbContext.ToListAsync());
        }

        // GET: ShiftReports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shiftReport = await _context.ShiftReports
                .Include(s => s.Khadem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shiftReport == null)
            {
                return NotFound();
            }

            return View(shiftReport);
        }

        // GET: ShiftReports/Create
        public IActionResult Create()
        {
            ViewData["KhademId"] = new SelectList(_context.Khadems, "Id", "FirstName");
            return View();
        }

        // POST: ShiftReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Shift,Subject,QuranActivity,AhkamActivity,GameActivity,PoemActivity,StoryActivity,CraftActivity,Description,TotalParticipants,RegularCount,TravelerCount,PresentKhads,KhademId")] ShiftReport shiftReport,string ReportDate)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(ReportDate))
                {
                    var EnReportDate = ToEnglishNumbers(ReportDate);
                    // تبدیل رشته شمسی به میلادی و انتساب به مدل
                    shiftReport.ReportDate = EnReportDate.ToEn();
                }

                _context.Add(shiftReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KhademId"] = new SelectList(_context.Khadems, "Id", "FirstName", shiftReport.KhademId);
            return View(shiftReport);
        }

        // GET: ShiftReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shiftReport = await _context.ShiftReports.FindAsync(id);
            if (shiftReport == null)
            {
                return NotFound();
            }
            ViewData["KhademId"] = new SelectList(_context.Khadems, "Id", "FirstName", shiftReport.KhademId);
            return View(shiftReport);
        }

        // POST: ShiftReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReportDate,Shift,Subject,QuranActivity,AhkamActivity,GameActivity,PoemActivity,StoryActivity,CraftActivity,Description,TotalParticipants,RegularCount,TravelerCount,PresentKhads,KhademId")] ShiftReport shiftReport)
        {
            if (id != shiftReport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shiftReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShiftReportExists(shiftReport.Id))
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
            ViewData["KhademId"] = new SelectList(_context.Khadems, "Id", "FirstName", shiftReport.KhademId);
            return View(shiftReport);
        }

        // GET: ShiftReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shiftReport = await _context.ShiftReports
                .Include(s => s.Khadem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shiftReport == null)
            {
                return NotFound();
            }

            return View(shiftReport);
        }

        // POST: ShiftReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shiftReport = await _context.ShiftReports.FindAsync(id);
            if (shiftReport != null)
            {
                _context.ShiftReports.Remove(shiftReport);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public static string ToEnglishNumbers(string input)
        {
            return input.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2")
                .Replace("۳", "3").Replace("۴", "4").Replace("۵", "5")
                .Replace("۶", "6").Replace("۷", "7").Replace("۸", "8");
        }
        private bool ShiftReportExists(int id)
        {
            return _context.ShiftReports.Any(e => e.Id == id);
        }
    }
}
