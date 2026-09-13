using System.Security.Claims;
using DataAccess.Data;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminGonbadTala.Controllers;

[Authorize(Roles = UserRoles.Management + "," + UserRoles.ShiftLead)]
public class KhademAttendancesController : Controller
{
    private readonly GonbadDbContext _context;
    public KhademAttendancesController(GonbadDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var userId = CurrentKhademId();
        var query = _context.KhademAttendances.Include(item => item.Khadem).Include(item => item.RecordedByKhadem).AsQueryable();
        if (!User.IsInRole(UserRoles.Management)) query = query.Where(item => item.Khadem.ShiftLeadId == userId);
        return View(await query.OrderByDescending(item => item.AttendanceDate).ToListAsync());
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
        var query = _context.Khadems.Where(item => item.Role == UserRoles.Servant);
        if (!User.IsInRole(UserRoles.Management)) query = query.Where(item => item.ShiftLeadId == userId);
        ViewBag.Servants = await query.OrderBy(item => item.FirstName).ToListAsync();
    }
    private async Task<bool> CanRecordFor(int khademId) => User.IsInRole(UserRoles.Management) || await _context.Khadems.AnyAsync(item => item.Id == khademId && item.ShiftLeadId == CurrentKhademId());
    private int CurrentKhademId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
