using System.Security.Claims;
using DataAccess.Data;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminGonbadTala.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly GonbadDbContext _context;
    public ProfileController(GonbadDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var khademId = CurrentKhademId();
        var attendance = await _context.KhademAttendances.Where(item => item.KhademId == khademId)
            .OrderByDescending(item => item.AttendanceDate).ToListAsync();
        var leaves = await _context.LeaveRequests.Where(item => item.KhademId == khademId)
            .OrderByDescending(item => item.LeaveDate).ToListAsync();
        ViewBag.Leaves = leaves;
        return View(attendance);
    }

    [HttpGet]
    public IActionResult CreateLeave() => View(new LeaveRequest { LeaveDate = DateTime.Today });

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateLeave([Bind("LeaveDate,Description")] LeaveRequest request)
    {
        var khademId = CurrentKhademId();
        var khadem = await _context.Khadems.FindAsync(khademId);
        if (khadem?.ShiftLeadId == null) ModelState.AddModelError(string.Empty, "برای شما سرشیفت تعیین نشده است؛ با مدیریت تماس بگیرید.");
        if (request.LeaveDate.Date < DateTime.Today) ModelState.AddModelError(nameof(request.LeaveDate), "تاریخ مرخصی باید امروز یا بعد از امروز باشد.");
        if (!ModelState.IsValid) return View(request);

        request.KhademId = khademId;
        request.ShiftLeadId = khadem!.ShiftLeadId;
        request.LeaveDate = request.LeaveDate.Date;
        _context.LeaveRequests.Add(request);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private int CurrentKhademId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
