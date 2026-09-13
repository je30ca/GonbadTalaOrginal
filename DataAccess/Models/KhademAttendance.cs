using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public class KhademAttendance
{
    public int Id { get; set; }

    [Display(Name = "تاریخ حضور")]
    public DateTime AttendanceDate { get; set; }

    [Display(Name = "زمان ورود")]
    public DateTime? EntryTime { get; set; }

    [Display(Name = "زمان خروج")]
    public DateTime? ExitTime { get; set; }

    [Display(Name = "توضیحات")]
    public string? Notes { get; set; }

    public int KhademId { get; set; }
    public Khadem Khadem { get; set; } = null!;

    public int RecordedByKhademId { get; set; }
    public Khadem RecordedByKhadem { get; set; } = null!;
}
