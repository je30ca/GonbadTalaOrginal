using System.Security.Claims;
using System.Globalization;
using DataAccess.Data;
using DataAccess.Models;
using AdminGonbadTala.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminGonbadTala.Controllers;

[Authorize(Roles = UserRoles.Management + "," + UserRoles.ShiftLead)]
public class KhademAttendancesController : Controller
{
    private readonly GonbadDbContext _context;
    public KhademAttendancesController(GonbadDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? date)
    {
        var userId = CurrentKhademId();
        var selectedDate = ParseDate(date) ?? DateTime.Today;
        var peopleQuery = _context.Khadems.AsQueryable();
        if (!User.IsInRole(UserRoles.Management)) peopleQuery = peopleQuery.Where(item => item.ShiftLeadId == userId);
        var people = await peopleQuery.OrderBy(item => item.FirstName).ThenBy(item => item.LastName).ToListAsync();
        var ids = people.Select(item => item.Id).ToList();
        var records = await _context.KhademAttendances.Where(item => ids.Contains(item.KhademId) && item.AttendanceDate == selectedDate).ToDictionaryAsync(item => item.KhademId);
        var rows = new List<KhademAttendanceRowViewModel>();
        foreach (var person in people)
        {
            records.TryGetValue(person.Id, out var record);
            rows.Add(new KhademAttendanceRowViewModel
            {
                KhademId = person.Id,
                KhademName = person.FullName,
                EntryTime = record?.EntryTime?.ToString("HH:mm"),
                ExitTime = record?.ExitTime?.ToString("HH:mm"),
                Notes = record?.Notes
            });
        }
        var model = new KhademAttendanceDayViewModel { Date = selectedDate, Rows = rows };
        return View("Index", model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDay(KhademAttendanceDayViewModel model)
    {
        var userId = CurrentKhademId();
        var allowedIds = await _context.Khadems.Where(item => User.IsInRole(UserRoles.Management) || item.ShiftLeadId == userId).Select(item => item.Id).ToListAsync();
        var date = model.Date.Date;
        foreach (var row in model.Rows.Where(row => allowedIds.Contains(row.KhademId)))
        {
            var entry = ParseTime(date, row.EntryTime);
            var exit = ParseTime(date, row.ExitTime);
            if (exit.HasValue && entry.HasValue && exit < entry) { ModelState.AddModelError(string.Empty, $"زمان خروج {row.KhademName} قبل از ورود است."); continue; }
            var record = await _context.KhademAttendances.FirstOrDefaultAsync(item => item.KhademId == row.KhademId && item.AttendanceDate == date);
            var hasData = entry.HasValue || exit.HasValue || !string.IsNullOrWhiteSpace(row.Notes);
            if (!hasData) { if (record != null) _context.KhademAttendances.Remove(record); continue; }
            record ??= new KhademAttendance { KhademId = row.KhademId, AttendanceDate = date, RecordedByKhademId = userId };
            record.EntryTime = entry; record.ExitTime = exit; record.Notes = row.Notes?.Trim();
            if (record.Id == 0) _context.KhademAttendances.Add(record);
        }
        if (!ModelState.IsValid) return await Index(date: model.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        await _context.SaveChangesAsync();
        TempData["AttendanceSaved"] = $"ورود و خروج روز {date:yyyy/MM/dd} ثبت شد.";
        return RedirectToAction(nameof(Index), new { date = date.ToString("yyyy-MM-dd") });
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadServants();
        return View(new KhademAttendance { AttendanceDate = DateTime.Today });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("KhademId,AttendanceDate,EntryTime,ExitTime,Notes")] KhademAttendance attendance)
    {
        if (!await CanRecordFor(attendance.KhademId)) return Forbid();
        attendance.AttendanceDate = attendance.AttendanceDate.Date;
        if (attendance.EntryTime.HasValue) attendance.EntryTime = attendance.AttendanceDate.Add(attendance.EntryTime.Value.TimeOfDay);
        if (attendance.ExitTime.HasValue) attendance.ExitTime = attendance.AttendanceDate.Add(attendance.ExitTime.Value.TimeOfDay);
        if (attendance.ExitTime < attendance.EntryTime) ModelState.AddModelError(nameof(attendance.ExitTime), "زمان خروج نمی‌تواند قبل از ورود باشد.");
        if (await _context.KhademAttendances.AnyAsync(item => item.KhademId == attendance.KhademId && item.AttendanceDate == attendance.AttendanceDate)) ModelState.AddModelError(string.Empty, "برای این خادم در این تاریخ، ورود و خروج ثبت شده است.");
        if (!ModelState.IsValid) { await LoadServants(); return View(attendance); }
        attendance.RecordedByKhademId = CurrentKhademId();
        _context.KhademAttendances.Add(attendance);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadServants()
    {
        var userId = CurrentKhademId();
        var query = _context.Khadems.AsQueryable();
        if (!User.IsInRole(UserRoles.Management)) query = query.Where(item => item.ShiftLeadId == userId);
        ViewBag.Servants = await query.OrderBy(item => item.FirstName).ToListAsync();
    }
    private async Task<bool> CanRecordFor(int khademId) => User.IsInRole(UserRoles.Management) || await _context.Khadems.AnyAsync(item => item.Id == khademId && item.ShiftLeadId == CurrentKhademId());
    private int CurrentKhademId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private static DateTime? ParseTime(DateTime date, string? value) => TimeSpan.TryParse(value, out var time) ? date.Add(time) : null;
    private static DateTime? ParseDate(string? value) => DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date.Date : null;
}
