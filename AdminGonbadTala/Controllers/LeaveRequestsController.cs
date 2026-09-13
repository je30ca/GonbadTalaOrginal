using System.Security.Claims;
using DataAccess.Data;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminGonbadTala.Controllers;

[Authorize]
public class LeaveRequestsController : Controller
{
    private readonly GonbadDbContext _context;
    public LeaveRequestsController(GonbadDbContext context) => _context = context;

    [Authorize(Roles = UserRoles.ShiftLead)]
    public async Task<IActionResult> ShiftLeadInbox() => View(await _context.LeaveRequests.Include(item => item.Khadem)
        .Where(item => item.ShiftLeadId == CurrentKhademId()).OrderBy(item => item.LeaveDate).ToListAsync());

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = UserRoles.ShiftLead)]
    public async Task<IActionResult> Forward(int id, string? comment)
    {
        var request = await _context.LeaveRequests.FirstOrDefaultAsync(item => item.Id == id && item.ShiftLeadId == CurrentKhademId());
        if (request == null || request.Status != LeaveRequestStatuses.PendingShiftLead) return NotFound();
        request.Status = LeaveRequestStatuses.PendingManagement; request.ShiftLeadComment = comment; request.SentToManagementAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(); return RedirectToAction(nameof(ShiftLeadInbox));
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = UserRoles.ShiftLead)]
    public async Task<IActionResult> AnnounceToKhadem(int id)
    {
        var request = await _context.LeaveRequests.FirstOrDefaultAsync(item => item.Id == id && item.ShiftLeadId == CurrentKhademId());
        if (request == null) return NotFound();

        request.Status = request.Status == LeaveRequestStatuses.ApprovedAwaitingShiftLeadNotification
            ? LeaveRequestStatuses.Approved
            : request.Status == LeaveRequestStatuses.RejectedAwaitingShiftLeadNotification
                ? LeaveRequestStatuses.Rejected
                : request.Status;

        if (request.Status is not (LeaveRequestStatuses.Approved or LeaveRequestStatuses.Rejected)) return BadRequest();
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(ShiftLeadInbox));
    }

    [Authorize(Roles = UserRoles.Management)]
    public async Task<IActionResult> ManagementInbox() => View(await _context.LeaveRequests.Include(item => item.Khadem).Include(item => item.ShiftLead)
        .Where(item => item.Status == LeaveRequestStatuses.PendingManagement).OrderBy(item => item.LeaveDate).ToListAsync());

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = UserRoles.Management)]
    public async Task<IActionResult> Decide(int id, bool approved, string? comment)
    {
        var request = await _context.LeaveRequests.FindAsync(id);
        if (request == null || request.Status != LeaveRequestStatuses.PendingManagement) return NotFound();
        request.Status = approved
            ? LeaveRequestStatuses.ApprovedAwaitingShiftLeadNotification
            : LeaveRequestStatuses.RejectedAwaitingShiftLeadNotification;
        request.ManagementComment = comment; request.FinalizedAt = DateTime.UtcNow; request.FinalizedByKhademId = CurrentKhademId();
        await _context.SaveChangesAsync(); return RedirectToAction(nameof(ManagementInbox));
    }
    private int CurrentKhademId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
