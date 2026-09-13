using System.Security.Claims;
using AdminGonbadTala.Models;
using DataAccess.Data;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminGonbadTala.Controllers;

[Authorize]
public class QasedakReportsController : Controller
{
    private readonly GonbadDbContext _context;

    public QasedakReportsController(GonbadDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var reports = await _context.QasedakReports
            .Include(report => report.Khadems)
            .ThenInclude(item => item.Khadem)
            .OrderByDescending(report => report.ExecutionDate)
            .ThenByDescending(report => report.Id)
            .ToListAsync();

        return View(reports);
    }

    [HttpGet]
    public async Task<IActionResult> Create(DateTime executionDate, string shift)
    {
        if (!IsValidShift(shift)) return BadRequest();

        var existing = await _context.QasedakReports
            .FirstOrDefaultAsync(report => report.ExecutionDate.Date == executionDate.Date && report.Shift == shift);
        if (existing != null)
        {
            TempData["QasedakMessage"] = "گزارش قاصدک این شیفت قبلاً ثبت شده است.";
            return RedirectToAction("Index", "TimeSheets", new { shift });
        }

        await LoadKhadems();
        return View(new QasedakReportCreateViewModel { ExecutionDate = executionDate.Date, Shift = shift });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(QasedakReportCreateViewModel model)
    {
        if (!IsValidShift(model.Shift)) ModelState.AddModelError(nameof(model.Shift), "شیفت نامعتبر است");
        if (model.KhademIds.Count == 0) ModelState.AddModelError(nameof(model.KhademIds), "حداقل یک خادم را انتخاب کنید");

        if (await _context.QasedakReports.AnyAsync(report => report.ExecutionDate.Date == model.ExecutionDate.Date && report.Shift == model.Shift))
            ModelState.AddModelError(string.Empty, "گزارش قاصدک این شیفت قبلاً ثبت شده است.");

        if (!ModelState.IsValid)
        {
            await LoadKhadems();
            return View(model);
        }

        var report = new QasedakReport
        {
            ExecutionDate = model.ExecutionDate.Date,
            Shift = model.Shift,
            Subject = model.Subject.Trim(),
            ExecutionCount = model.ExecutionCount,
            PresentedCircles = model.PresentedCircles.Trim(),
            ExecutionLocations = model.ExecutionLocations.Trim(),
            AudienceCount = model.AudienceCount,
            AgeGroup = model.AgeGroup.Trim(),
            CreatedByKhademId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var khademId) ? khademId : null,
            Khadems = model.KhademIds.Distinct().Select(id => new QasedakReportKhadem { KhademId = id }).ToList()
        };

        _context.QasedakReports.Add(report);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "TimeSheets", new { shift = model.Shift });
    }

    private async Task LoadKhadems()
    {
        ViewBag.Khadems = await _context.Khadems
            .OrderBy(item => item.FirstName).ThenBy(item => item.LastName)
            .ToListAsync();
    }

    private static bool IsValidShift(string shift) => shift is "morning" or "evening";
}
