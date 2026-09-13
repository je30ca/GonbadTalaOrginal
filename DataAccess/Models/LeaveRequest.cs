using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public static class LeaveRequestStatuses
{
    public const string PendingShiftLead = "در انتظار سرشیفت";
    public const string PendingManagement = "در انتظار مدیریت";
    public const string ApprovedAwaitingShiftLeadNotification = "تأیید مدیریت؛ در انتظار اعلام سرشیفت";
    public const string RejectedAwaitingShiftLeadNotification = "رد مدیریت؛ در انتظار اعلام سرشیفت";
    public const string Approved = "تأیید شد";
    public const string Rejected = "رد شد";
}

public class LeaveRequest
{
    public int Id { get; set; }

    [Display(Name = "تاریخ مرخصی")]
    public DateTime LeaveDate { get; set; }

    [Required, Display(Name = "توضیحات")]
    public string Description { get; set; } = string.Empty;

    public string Status { get; set; } = LeaveRequestStatuses.PendingShiftLead;
    public string? ShiftLeadComment { get; set; }
    public string? ManagementComment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SentToManagementAt { get; set; }
    public DateTime? FinalizedAt { get; set; }

    public int KhademId { get; set; }
    public Khadem Khadem { get; set; } = null!;
    public int? ShiftLeadId { get; set; }
    public Khadem? ShiftLead { get; set; }
    public int? FinalizedByKhademId { get; set; }
    public Khadem? FinalizedByKhadem { get; set; }
}
